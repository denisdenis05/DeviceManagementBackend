using DeviceManagement.Data.Models.Devices;

namespace DeviceManagement.Business.Services.Devices;

public interface IDevicesService
{
    Task<IEnumerable<DeviceDto>> RetrieveAllDevicesAsync();
    Task<DeviceDto> RetrieveDeviceByIdAsync(string identifier);
    Task AddDeviceAsync(DeviceDto device);
    Task EditDeviceAsync(DeviceDto device);
    Task DeleteDeviceAsync(string identifier);
}
