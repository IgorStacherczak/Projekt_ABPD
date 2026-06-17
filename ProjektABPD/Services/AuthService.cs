using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjektABPD.Data;
using ProjektABPD.DTOs;
using ProjektABPD.Exceptions;
using ProjektABPD.Models;

namespace ProjektABPD.Services;

public class AuthService : IAuthService
{
    private readonly DatabaseContext _context;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<Employee> _passwordHasher = new();

    public AuthService(DatabaseContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Login == dto.Login);

        if (employee == null)
            throw new BadRequestException("Invalid login or password.");

        var result = _passwordHasher.VerifyHashedPassword(
            employee,
            employee.Password,
            dto.Password
        );

        if (result == PasswordVerificationResult.Failed)
            throw new BadRequestException("Invalid login or password.");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, employee.IdEmployee.ToString()),
            new Claim(ClaimTypes.Name, employee.Login),
            new Claim(ClaimTypes.Role, employee.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }
}