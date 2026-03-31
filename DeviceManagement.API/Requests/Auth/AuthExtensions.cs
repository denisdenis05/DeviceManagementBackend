using DeviceManagement.Business.Models.Auth;

namespace DeviceManagement.API.Requests.Auth;

public static class AuthExtensions
{
    public static RegisterDto ToRegisterDto(this RegisterRequest request)
    {
        return new RegisterDto
        {
            Email = request.Email,
            Password = request.Password
        };
    }

    public static LoginDto ToLoginDto(this LoginRequest request)
    {
        return new LoginDto
        {
            Email = request.Email,
            Password = request.Password
        };
    }
}
