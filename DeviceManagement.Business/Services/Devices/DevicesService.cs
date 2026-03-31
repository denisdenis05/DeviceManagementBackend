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

    public async Task<DeviceDto> RetrieveDeviceByIdAsync(string identifier)
    {
        return await _devicesRepository.RetrieveDeviceByIdAsync(identifier);
    }

    public async Task AddDeviceAsync(DeviceDto device)
    {
        await _devicesRepository.InsertDeviceAsync(device);
    }

    public async Task EditDeviceAsync(DeviceDto device)
    {
        await _devicesRepository.UpdateDeviceAsync(device);
    }

    public async Task DeleteDeviceAsync(string identifier)
    {
        await _devicesRepository.DeleteDeviceAsync(identifier);
    }
}
