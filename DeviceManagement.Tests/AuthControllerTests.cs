using DeviceManagement.API.Controllers;
using DeviceManagement.API.Requests.Auth;
using DeviceManagement.Business.Models.Auth;
using DeviceManagement.Business.Services.Auth;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DeviceManagement.Tests;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _authController;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _authController = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Register_WhenAuthServiceSucceeds_ShouldReturnOkWithResult()
    {
        const string userEmail = "test@example.com";
        const string userPassword = "password";
        const string jwtToken = "token";
        var registerRequest = new RegisterRequest { Email = userEmail, Password = userPassword };
        var authResultDto = new AuthResultDto { Token = jwtToken };
        _authServiceMock.Setup(service => service.RegisterAsync(It.IsAny<RegisterDto>()))
            .ReturnsAsync(authResultDto);

        var actionResult = await _authController.Register(registerRequest);

        actionResult.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult as OkObjectResult;
        okResult!.Value.Should().Be(authResultDto);
    }

    [Fact]
    public async Task Register_WhenAuthServiceThrowsException_ShouldReturnBadRequest()
    {
        var registerRequest = new RegisterRequest { Email = "fail@example.com", Password = "password" };
        _authServiceMock.Setup(service => service.RegisterAsync(It.IsAny<RegisterDto>()))
            .ThrowsAsync(new Exception("Fail"));

        var actionResult = await _authController.Register(registerRequest);

        actionResult.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task Login_WhenAuthServiceSucceeds_ShouldReturnOkWithResult()
    {
        const string userEmail = "test@example.com";
        const string userPassword = "password";
        const string jwtToken = "token";
        var loginRequest = new LoginRequest { Email = userEmail, Password = userPassword };
        var authResultDto = new AuthResultDto { Token = jwtToken };
        _authServiceMock.Setup(service => service.LoginAsync(It.IsAny<LoginDto>()))
            .ReturnsAsync(authResultDto);

        var actionResult = await _authController.Login(loginRequest);

        actionResult.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult as OkObjectResult;
        okResult!.Value.Should().Be(authResultDto);
    }

    [Fact]
    public async Task Login_WhenAuthServiceThrowsException_ShouldReturnUnauthorized()
    {
        var loginRequest = new LoginRequest { Email = "fail@example.com", Password = "password" };
        _authServiceMock.Setup(service => service.LoginAsync(It.IsAny<LoginDto>()))
            .ThrowsAsync(new Exception("Fail"));

        var actionResult = await _authController.Login(loginRequest);

        actionResult.Should().BeOfType<UnauthorizedResult>();
    }
}
