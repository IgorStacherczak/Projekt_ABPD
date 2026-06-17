using Microsoft.AspNetCore.Mvc;
using ProjektABPD.DTOs;
using ProjektABPD.Exceptions;
using ProjektABPD.Services;

namespace ProjektABPD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        try
        {
            var response = await _authService.LoginAsync(dto);
            return Ok(response);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}