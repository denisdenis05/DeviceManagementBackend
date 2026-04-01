using DeviceManagement.Data.Models.Devices;

namespace DeviceManagement.Business.Services.AI;

public interface IAiService
{
    Task<string> GenerateDeviceDescriptionAsync(DeviceDto device);
}
