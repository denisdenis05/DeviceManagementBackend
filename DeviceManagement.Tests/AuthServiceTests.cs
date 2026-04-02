using DeviceManagement.Business.Models.Auth;
using DeviceManagement.Business.Services.Auth;
using DeviceManagement.Data;
using DeviceManagement.Data.Models.Auth;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DeviceManagement.Tests;

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authRepositoryMock = new Mock<IAuthRepository>();
        _configurationMock = new Mock<IConfiguration>();
        _authService = new AuthService(_authRepositoryMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_WhenUserAlreadyExists_ShouldThrowException()
    {
        const string userEmail = "existing@example.com";
        var registerDto = new RegisterDto { Email = userEmail, Password = "password" };
        _authRepositoryMock.Setup(repository => repository.RetrieveUserByEmailAsync(userEmail))
            .ReturnsAsync(new UserDto { Email = userEmail });

        var action = async () => await _authService.RegisterAsync(registerDto);

        await action.Should().ThrowAsync<Exception>().WithMessage("User already exists.");
    }

    [Fact]
    public async Task RegisterAsync_WhenUserDoesNotExist_ShouldInsertUserAndReturnToken()
    {
        const string userEmail = "new@example.com";
        var registerDto = new RegisterDto { Email = userEmail, Password = "password" };
        _authRepositoryMock.Setup(repository => repository.RetrieveUserByEmailAsync(userEmail))
            .ReturnsAsync((UserDto?)null);
        _authRepositoryMock.Setup(repository => repository.InsertUserAsync(It.IsAny<UserDto>()))
            .Callback<UserDto>(user => user.Id = "newId")
            .Returns(Task.CompletedTask);
        SetupJwtSettings();

        var authResult = await _authService.RegisterAsync(registerDto);

        authResult.Token.Should().NotBeNullOrEmpty();
        _authRepositoryMock.Verify(repository => repository.InsertUserAsync(It.Is<UserDto>(u => u.Email == userEmail)), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WhenUserDoesNotExist_ShouldThrowException()
    {
        const string userEmail = "missing@example.com";
        var loginDto = new LoginDto { Email = userEmail, Password = "password" };
        _authRepositoryMock.Setup(repository => repository.RetrieveUserByEmailAsync(userEmail))
            .ReturnsAsync((UserDto?)null);

        var action = async () => await _authService.LoginAsync(loginDto);

        await action.Should().ThrowAsync<Exception>().WithMessage("Invalid email or password.");
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsInvalid_ShouldThrowException()
    {
        const string userEmail = "test@example.com";
        const string correctPassword = "correct";
        const string wrongPassword = "wrong";
        var loginDto = new LoginDto { Email = userEmail, Password = wrongPassword };
        var existingUser = new UserDto { Email = userEmail, PasswordHash = BCrypt.Net.BCrypt.HashPassword(correctPassword) };
        _authRepositoryMock.Setup(repository => repository.RetrieveUserByEmailAsync(userEmail))
            .ReturnsAsync(existingUser);

        var action = async () => await _authService.LoginAsync(loginDto);

        await action.Should().ThrowAsync<Exception>().WithMessage("Invalid email or password.");
    }

    [Fact]
    public async Task LoginAsync_WhenCredentialsCorrect_ShouldReturnToken()
    {
        const string userEmail = "test@example.com";
        const string password = "password";
        var loginDto = new LoginDto { Email = userEmail, Password = password };
        var existingUser = new UserDto { Id = "1", Email = userEmail, PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) };
        _authRepositoryMock.Setup(repository => repository.RetrieveUserByEmailAsync(userEmail))
            .ReturnsAsync(existingUser);
        SetupJwtSettings();

        var authResult = await _authService.LoginAsync(loginDto);

        authResult.Token.Should().NotBeNullOrEmpty();
    }

    private void SetupJwtSettings()
    {
        var jwtSettingsMock = new Mock<IConfigurationSection>();
        jwtSettingsMock.Setup(section => section["Key"]).Returns("SuperSecretKeyForTestingPurposesOnly");
        jwtSettingsMock.Setup(section => section["ExpiryInDays"]).Returns("7");
        jwtSettingsMock.Setup(section => section["Issuer"]).Returns("TestIssuer");
        jwtSettingsMock.Setup(section => section["Audience"]).Returns("TestAudience");
        _configurationMock.Setup(configuration => configuration.GetSection("JwtSettings")).Returns(jwtSettingsMock.Object);
    }
}
