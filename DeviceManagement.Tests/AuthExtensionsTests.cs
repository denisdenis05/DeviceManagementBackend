using DeviceManagement.API.Requests.Auth;
using FluentAssertions;

namespace DeviceManagement.Tests;

public class AuthExtensionsTests
{
    [Fact]
    public void ToRegisterDto_WhenPassedValidRegisterRequest_ShouldReturnMatchingRegisterDto()
    {
        const string userEmail = "test@example.com";
        const string userPassword = "securePassword";
        var registerRequest = new RegisterRequest { Email = userEmail, Password = userPassword };

        var registerDto = registerRequest.ToRegisterDto();

        registerDto.Email.Should().Be(userEmail);
        registerDto.Password.Should().Be(userPassword);
    }

    [Fact]
    public void ToLoginDto_WhenPassedValidLoginRequest_ShouldReturnMatchingLoginDto()
    {
        const string userEmail = "test@example.com";
        const string userPassword = "securePassword";
        var loginRequest = new LoginRequest { Email = userEmail, Password = userPassword };

        var loginDto = loginRequest.ToLoginDto();

        loginDto.Email.Should().Be(userEmail);
        loginDto.Password.Should().Be(userPassword);
    }
}
