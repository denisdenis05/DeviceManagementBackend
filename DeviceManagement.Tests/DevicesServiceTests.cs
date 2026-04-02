using DeviceManagement.Business.Services.Devices;
using DeviceManagement.Data;
using DeviceManagement.Data.Models.Devices;
using FluentAssertions;
using Moq;

namespace DeviceManagement.Tests;

public class DevicesServiceTests
{
    private readonly Mock<IDevicesRepository> _devicesRepositoryMock;
    private readonly DevicesService _devicesService;

    public DevicesServiceTests()
    {
        _devicesRepositoryMock = new Mock<IDevicesRepository>();
        _devicesService = new DevicesService(_devicesRepositoryMock.Object);
    }

    [Fact]
    public async Task RetrieveAllDevicesAsync_WhenCalled_ShouldDelegateToRepository()
    {
        var devices = new List<DeviceDto> { new DeviceDto { Id = "1" } };
        _devicesRepositoryMock.Setup(repository => repository.RetrieveAllDevicesAsync())
            .ReturnsAsync(devices);

        var result = await _devicesService.RetrieveAllDevicesAsync();

        result.Should().BeEquivalentTo(devices);
    }

    [Fact]
    public async Task RetrieveDeviceByIdAsync_WhenCalled_ShouldDelegateToRepository()
    {
        const string deviceIdentifier = "123";
        var deviceDto = new DeviceDto { Id = deviceIdentifier };
        _devicesRepositoryMock.Setup(repository => repository.RetrieveDeviceByIdAsync(deviceIdentifier))
            .ReturnsAsync(deviceDto);

        var result = await _devicesService.RetrieveDeviceByIdAsync(deviceIdentifier);

        result.Should().Be(deviceDto);
    }

    [Fact]
    public async Task AddDeviceAsync_WhenCalled_ShouldDelegateToRepository()
    {
        var deviceDto = new DeviceDto { Name = "New" };

        await _devicesService.AddDeviceAsync(deviceDto);

        _devicesRepositoryMock.Verify(repository => repository.InsertDeviceAsync(deviceDto), Times.Once);
    }

    [Fact]
    public async Task EditDeviceAsync_WhenCalled_ShouldDelegateToRepository()
    {
        const string deviceIdentifier = "1";
        const string assignedUserId = "user123";
        const string assignedUserEmail = "test@example.com";
        
        var existingDevice = new DeviceDto 
        { 
            Id = deviceIdentifier, 
            Name = "Original", 
            AssignedUserId = assignedUserId, 
            AssignedUserEmail = assignedUserEmail 
        };
        
        var updateRequest = new DeviceDto 
        { 
            Id = deviceIdentifier, 
            Name = "Updated", 
            Manufacturer = "New Manufacturer" 
        };

        _devicesRepositoryMock.Setup(repository => repository.RetrieveDeviceByIdAsync(deviceIdentifier))
            .ReturnsAsync(existingDevice);

        await _devicesService.EditDeviceAsync(updateRequest);

        existingDevice.Name.Should().Be(updateRequest.Name);
        existingDevice.Manufacturer.Should().Be(updateRequest.Manufacturer);
        existingDevice.AssignedUserId.Should().Be(assignedUserId);
        existingDevice.AssignedUserEmail.Should().Be(assignedUserEmail);
        _devicesRepositoryMock.Verify(repository => repository.UpdateDeviceAsync(existingDevice), Times.Once);
    }

    [Fact]
    public async Task DeleteDeviceAsync_WhenCalled_ShouldDelegateToRepository()
    {
        const string deviceIdentifier = "1";

        await _devicesService.DeleteDeviceAsync(deviceIdentifier);

        _devicesRepositoryMock.Verify(repository => repository.DeleteDeviceAsync(deviceIdentifier), Times.Once);
    }

    [Fact]
    public async Task AssignDeviceAsync_WhenDeviceAlreadyHasAssignee_ShouldThrowException()
    {
        const string deviceIdentifier = "1";
        var existingDevice = new DeviceDto { Id = deviceIdentifier, AssignedUserId = "someoneElse" };
        _devicesRepositoryMock.Setup(repository => repository.RetrieveDeviceByIdAsync(deviceIdentifier))
            .ReturnsAsync(existingDevice);

        var action = async () => await _devicesService.AssignDeviceAsync(deviceIdentifier, "user123", "email");

        await action.Should().ThrowAsync<Exception>().WithMessage("Device is already assigned.");
    }

    [Fact]
    public async Task AssignDeviceAsync_WhenDeviceIsFree_ShouldUpdateWithUserInfoAndDelegateToRepository()
    {
        const string deviceIdentifier = "1";
        const string userIdentifier = "user123";
        const string userEmail = "test@example.com";
        var existingDevice = new DeviceDto { Id = deviceIdentifier, AssignedUserId = string.Empty };
        _devicesRepositoryMock.Setup(repository => repository.RetrieveDeviceByIdAsync(deviceIdentifier))
            .ReturnsAsync(existingDevice);

        await _devicesService.AssignDeviceAsync(deviceIdentifier, userIdentifier, userEmail);

        existingDevice.AssignedUserId.Should().Be(userIdentifier);
        existingDevice.AssignedUserEmail.Should().Be(userEmail);
        _devicesRepositoryMock.Verify(repository => repository.UpdateDeviceAsync(existingDevice), Times.Once);
    }

    [Fact]
    public async Task UnassignDeviceAsync_WhenUserIsNotAssignee_ShouldThrowException()
    {
        const string deviceIdentifier = "1";
        var existingDevice = new DeviceDto { Id = deviceIdentifier, AssignedUserId = "differentUser" };
        _devicesRepositoryMock.Setup(repository => repository.RetrieveDeviceByIdAsync(deviceIdentifier))
            .ReturnsAsync(existingDevice);

        var action = async () => await _devicesService.UnassignDeviceAsync(deviceIdentifier, "user123");

        await action.Should().ThrowAsync<Exception>().WithMessage("You are not authorized to unassign this device.");
    }

    [Fact]
    public async Task UnassignDeviceAsync_WhenUserIsAssignee_ShouldClearAssignmentAndDelegateToRepository()
    {
        const string deviceIdentifier = "1";
        const string userIdentifier = "user123";
        var existingDevice = new DeviceDto { Id = deviceIdentifier, AssignedUserId = userIdentifier };
        _devicesRepositoryMock.Setup(repository => repository.RetrieveDeviceByIdAsync(deviceIdentifier))
            .ReturnsAsync(existingDevice);

        await _devicesService.UnassignDeviceAsync(deviceIdentifier, userIdentifier);

        existingDevice.AssignedUserId.Should().BeEmpty();
        existingDevice.AssignedUserEmail.Should().BeEmpty();
        _devicesRepositoryMock.Verify(repository => repository.UpdateDeviceAsync(existingDevice), Times.Once);
    }
}
