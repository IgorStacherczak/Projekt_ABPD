using ProjektABPD.DTOs;

namespace ProjektABPD.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
}