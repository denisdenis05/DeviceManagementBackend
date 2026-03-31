using DeviceManagement.Business.Models.Auth;

namespace DeviceManagement.Business.Services.Auth;

public interface IAuthService
{
    Task<AuthResultDto> RegisterAsync(RegisterDto request);
    Task<AuthResultDto> LoginAsync(LoginDto request);
}
