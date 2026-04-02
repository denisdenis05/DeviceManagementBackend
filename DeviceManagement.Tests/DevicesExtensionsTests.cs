using DeviceManagement.API.Requests.Devices;
using DeviceManagement.Data.Models.Devices;
using FluentAssertions;

namespace DeviceManagement.Tests;

public class DevicesExtensionsTests
{
    [Fact]
    public void ToAddDeviceDto_WhenPassedValidRequest_ShouldReturnMatchingDto()
    {
        var request = new AddDeviceRequest
        {
            Name = "Galaxy",
            Manufacturer = "Samsung",
            Type = "Phone",
            OperatingSystem = "Android",
            OsVersion = "13",
            Processor = "Exynos",
            RamAmount = 8,
            Description = "Desc"
        };

        var dto = request.ToAddDeviceDto();

        dto.Name.Should().Be(request.Name);
        dto.Manufacturer.Should().Be(request.Manufacturer);
        dto.Type.Should().Be(request.Type);
        dto.OperatingSystem.Should().Be(request.OperatingSystem);
        dto.OsVersion.Should().Be(request.OsVersion);
        dto.Processor.Should().Be(request.Processor);
        dto.RamAmount.Should().Be(request.RamAmount);
        dto.Description.Should().Be(request.Description);
        dto.Id.Should().BeEmpty();
    }

    [Fact]
    public void ToEditDeviceDto_WhenPassedValidRequest_ShouldReturnMatchingDtoWithId()
    {
        var request = new EditDeviceRequest
        {
            Identifier = "123",
            Name = "Galaxy",
            Manufacturer = "Samsung",
            Type = "Phone",
            OperatingSystem = "Android",
            OsVersion = "13",
            Processor = "Exynos",
            RamAmount = 8,
            Description = "Desc"
        };

        var dto = request.ToEditDeviceDto();

        dto.Id.Should().Be(request.Identifier);
        dto.Name.Should().Be(request.Name);
    }

    [Fact]
    public void ToAssignDeviceDto_WhenPassedRequest_ShouldUpdateDeviceAssignedUserId()
    {
        var device = new DeviceDto { Id = "1" };
        var request = new AssignDeviceRequest { UserIdentifier = "user123" };

        var updatedDevice = request.ToAssignDeviceDto(device);

        updatedDevice.AssignedUserId.Should().Be(request.UserIdentifier);
    }

    [Fact]
    public void ToUnassignDeviceDto_WhenPassedRequest_ShouldClearDeviceAssignedUserId()
    {
        var device = new DeviceDto { Id = "1", AssignedUserId = "user123" };
        var request = new UnassignDeviceRequest();

        var updatedDevice = request.ToUnassignDeviceDto(device);

        updatedDevice.AssignedUserId.Should().BeEmpty();
    }
}
