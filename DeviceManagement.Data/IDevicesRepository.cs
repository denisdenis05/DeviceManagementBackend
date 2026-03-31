using DeviceManagement.Data.Models.Devices;

namespace DeviceManagement.Data;

public interface IDevicesRepository
{
    Task<IEnumerable<DeviceDto>> RetrieveAllDevicesAsync();
    Task<DeviceDto> RetrieveDeviceByIdAsync(string identifier);
    Task InsertDeviceAsync(DeviceDto device);
    Task UpdateDeviceAsync(DeviceDto device);
    Task DeleteDeviceAsync(string identifier);
}
