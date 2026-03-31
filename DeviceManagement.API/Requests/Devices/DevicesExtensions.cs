using DeviceManagement.API.Requests.Devices;
using DeviceManagement.Data.Models.Devices;

namespace DeviceManagement.API.Requests.Devices;

public static class DevicesExtensions
{
    public static DeviceDto ToAddDeviceDto(this AddDeviceRequest request) =>
        new DeviceDto
        {
            Name = request.Name,
            Manufacturer = request.Manufacturer,
            Type = request.Type,
            OperatingSystem = request.OperatingSystem,
            OsVersion = request.OsVersion,
            Processor = request.Processor,
            RamAmount = request.RamAmount,
            Description = request.Description
        };

    public static DeviceDto ToEditDeviceDto(this EditDeviceRequest request) =>
        new DeviceDto
        {
            Id = request.Identifier,
            Name = request.Name,
            Manufacturer = request.Manufacturer,
            Type = request.Type,
            OperatingSystem = request.OperatingSystem,
            OsVersion = request.OsVersion,
            Processor = request.Processor,
            RamAmount = request.RamAmount,
            Description = request.Description
        };

    public static DeviceDto ToAssignDeviceDto(this AssignDeviceRequest request, DeviceDto existingDevice)
    {
        existingDevice.AssignedUserId = request.UserIdentifier;
        return existingDevice;
    }

    public static DeviceDto ToUnassignDeviceDto(this UnassignDeviceRequest request, DeviceDto existingDevice)
    {
        existingDevice.AssignedUserId = string.Empty;
        return existingDevice;
    }
}
