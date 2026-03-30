using DeviceManagement.Data;
using DeviceManagement.Data.Models.Devices;

namespace DeviceManagement.Business.Services.Devices;

public class DevicesService : IDevicesService
{
    private readonly IDevicesRepository _devicesRepository;

    public DevicesService(IDevicesRepository devicesRepository)
    {
        _devicesRepository = devicesRepository;
    }

    public async Task<IEnumerable<DeviceDto>> RetrieveAllDevicesAsync()
    {
        return await _devicesRepository.RetrieveAllDevicesAsync();
    }
}
