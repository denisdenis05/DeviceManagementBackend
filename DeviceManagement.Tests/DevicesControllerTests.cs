using DeviceManagement.API.Controllers;
using DeviceManagement.API.Requests.Devices;
using DeviceManagement.Business.Services.Devices;
using DeviceManagement.Data.Models.Devices;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace DeviceManagement.Tests;

public class DevicesControllerTests
{
    private readonly Mock<IDevicesService> _devicesServiceMock;
    private readonly DevicesController _devicesController;

    public DevicesControllerTests()
    {
        _devicesServiceMock = new Mock<IDevicesService>();
        _devicesController = new DevicesController(_devicesServiceMock.Object);
    }

    [Fact]
    public async Task RetrieveAllDevicesAsync_WhenServiceSucceeds_ShouldReturnOkWithDevices()
    {
        var devices = new List<DeviceDto> { new DeviceDto { Id = "1", Name = "Device" } };
        _devicesServiceMock.Setup(service => service.RetrieveAllDevicesAsync())
            .ReturnsAsync(devices);

        var actionResult = await _devicesController.RetrieveAllDevicesAsync();

        actionResult.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(devices);
    }

    [Fact]
    public async Task GetRequest_WhenDeviceExists_ShouldReturnOkWithDevice()
    {
        const string deviceIdentifier = "123";
        var deviceDto = new DeviceDto { Id = deviceIdentifier, Name = "Test Device" };
        _devicesServiceMock.Setup(service => service.RetrieveDeviceByIdAsync(deviceIdentifier))
            .ReturnsAsync(deviceDto);

        var actionResult = await _devicesController.GetRequest(deviceIdentifier);

        actionResult.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(deviceDto);
    }

    [Fact]
    public async Task GetRequest_WhenServiceThrowsException_ShouldReturnBadRequest()
    {
        _devicesServiceMock.Setup(service => service.RetrieveDeviceByIdAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception());

        var actionResult = await _devicesController.GetRequest("invalid");

        actionResult.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task AddRequest_WhenServiceSucceeds_ShouldReturnOk()
    {
        var addRequest = new AddDeviceRequest { Name = "New Device" };
        _devicesServiceMock.Setup(service => service.AddDeviceAsync(It.IsAny<DeviceDto>()))
            .Returns(Task.CompletedTask);

        var actionResult = await _devicesController.AddRequest(addRequest);

        actionResult.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task AddRequest_WhenServiceThrows_ShouldReturnBadRequest()
    {
        _devicesServiceMock.Setup(service => service.AddDeviceAsync(It.IsAny<DeviceDto>()))
            .ThrowsAsync(new Exception());

        var actionResult = await _devicesController.AddRequest(new AddDeviceRequest());

        actionResult.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task EditRequest_WhenServiceSucceeds_ShouldReturnOk()
    {
        var editRequest = new EditDeviceRequest { Id = "1", Name = "Updated" };
        _devicesServiceMock.Setup(service => service.EditDeviceAsync(It.IsAny<DeviceDto>()))
            .Returns(Task.CompletedTask);

        var actionResult = await _devicesController.EditRequest(editRequest);

        actionResult.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task EditRequest_WhenServiceThrows_ShouldReturnBadRequest()
    {
        _devicesServiceMock.Setup(service => service.EditDeviceAsync(It.IsAny<DeviceDto>()))
            .ThrowsAsync(new Exception());

        var actionResult = await _devicesController.EditRequest(new EditDeviceRequest());

        actionResult.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task DeleteRequest_WhenServiceSucceeds_ShouldReturnOk()
    {
        const string deviceIdentifier = "1";
        _devicesServiceMock.Setup(service => service.DeleteDeviceAsync(deviceIdentifier))
            .Returns(Task.CompletedTask);

        var actionResult = await _devicesController.DeleteRequest(deviceIdentifier);

        actionResult.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task DeleteRequest_WhenServiceThrows_ShouldReturnBadRequest()
    {
        _devicesServiceMock.Setup(service => service.DeleteDeviceAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception());

        var actionResult = await _devicesController.DeleteRequest("fail");

        actionResult.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task AssignDeviceRequest_WhenUserClaimsMissing_ShouldReturnUnauthorized()
    {
        SetupControllerContext(new ClaimsPrincipal(new ClaimsIdentity()));
        var assignRequest = new AssignDeviceRequest { DeviceIdentifier = "1" };

        var actionResult = await _devicesController.AssignDeviceRequest(assignRequest);

        actionResult.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task AssignDeviceRequest_WhenServiceSucceeds_ShouldReturnOk()
    {
        const string userIdentifier = "user123";
        const string userEmail = "test@example.com";
        const string deviceIdentifier = "device123";
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userIdentifier),
            new Claim(ClaimTypes.Email, userEmail)
        };
        SetupControllerContext(new ClaimsPrincipal(new ClaimsIdentity(claims)));
        var assignRequest = new AssignDeviceRequest { DeviceIdentifier = deviceIdentifier };
        _devicesServiceMock.Setup(service => service.AssignDeviceAsync(deviceIdentifier, userIdentifier, userEmail))
            .Returns(Task.CompletedTask);

        var actionResult = await _devicesController.AssignDeviceRequest(assignRequest);

        actionResult.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task AssignDeviceRequest_WhenServiceThrows_ShouldReturnBadRequest()
    {
        const string userIdentifier = "user123";
        const string userEmail = "test@example.com";
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userIdentifier),
            new Claim(ClaimTypes.Email, userEmail)
        };
        SetupControllerContext(new ClaimsPrincipal(new ClaimsIdentity(claims)));
        _devicesServiceMock.Setup(service => service.AssignDeviceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception());

        var actionResult = await _devicesController.AssignDeviceRequest(new AssignDeviceRequest());

        actionResult.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task UnassignDeviceRequest_WhenUserClaimsMissing_ShouldReturnUnauthorized()
    {
        SetupControllerContext(new ClaimsPrincipal(new ClaimsIdentity()));
        var unassignRequest = new UnassignDeviceRequest { DeviceIdentifier = "1" };

        var actionResult = await _devicesController.UnassignDeviceRequest(unassignRequest);

        actionResult.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task UnassignDeviceRequest_WhenServiceSucceeds_ShouldReturnOk()
    {
        const string userIdentifier = "user123";
        const string deviceIdentifier = "device123";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userIdentifier) };
        SetupControllerContext(new ClaimsPrincipal(new ClaimsIdentity(claims)));
        var unassignRequest = new UnassignDeviceRequest { DeviceIdentifier = deviceIdentifier };
        _devicesServiceMock.Setup(service => service.UnassignDeviceAsync(deviceIdentifier, userIdentifier))
            .Returns(Task.CompletedTask);

        var actionResult = await _devicesController.UnassignDeviceRequest(unassignRequest);

        actionResult.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task UnassignDeviceRequest_WhenServiceThrows_ShouldReturnBadRequest()
    {
        const string userIdentifier = "user123";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userIdentifier) };
        SetupControllerContext(new ClaimsPrincipal(new ClaimsIdentity(claims)));
        _devicesServiceMock.Setup(service => service.UnassignDeviceAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception());

        var actionResult = await _devicesController.UnassignDeviceRequest(new UnassignDeviceRequest());

        actionResult.Should().BeOfType<BadRequestResult>();
    }

    private void SetupControllerContext(ClaimsPrincipal claimsPrincipal)
    {
        _devicesController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }
}
