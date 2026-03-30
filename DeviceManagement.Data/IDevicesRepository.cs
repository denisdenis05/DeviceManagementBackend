using DeviceManagement.Data.Models.Devices;

namespace DeviceManagement.Data;

public interface IDevicesRepository
{
    Task<IEnumerable<DeviceDto>> RetrieveAllDevicesAsync();
}
