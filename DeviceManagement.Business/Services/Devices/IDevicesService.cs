using DeviceManagement.Data.Models.Devices;

namespace DeviceManagement.Business.Services.Devices;

public interface IDevicesService
{
    Task<IEnumerable<DeviceDto>> RetrieveAllDevicesAsync();
}
