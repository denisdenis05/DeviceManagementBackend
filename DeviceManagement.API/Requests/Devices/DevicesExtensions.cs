using DeviceManagement.API.Requests.Devices;
using DeviceManagement.Data.Models.Devices;

namespace DeviceManagement.API.Requests.Devices;

public static class DevicesExtensions
{
    public static DeviceDto ToDeviceDto(this DeviceRequest request) =>
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
}
