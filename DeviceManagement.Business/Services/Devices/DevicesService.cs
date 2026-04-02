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
        var existingDevice = await _devicesRepository.RetrieveDeviceByIdAsync(device.Id);
        
        if (existingDevice == null)
        {
            throw new Exception("Device not found.");
        }

        existingDevice.Name = device.Name;
        existingDevice.Manufacturer = device.Manufacturer;
        existingDevice.Type = device.Type;
        existingDevice.OperatingSystem = device.OperatingSystem;
        existingDevice.OsVersion = device.OsVersion;
        existingDevice.Processor = device.Processor;
        existingDevice.RamAmount = device.RamAmount;
        existingDevice.Description = device.Description;

        await _devicesRepository.UpdateDeviceAsync(existingDevice);
    }

    public async Task DeleteDeviceAsync(string identifier)
    {
        await _devicesRepository.DeleteDeviceAsync(identifier);
    }

    public async Task AssignDeviceAsync(string deviceIdentifier, string userIdentifier, string userEmail)
    {
        var device = await _devicesRepository.RetrieveDeviceByIdAsync(deviceIdentifier);
        
        if (!string.IsNullOrEmpty(device.AssignedUserId))
        {
            throw new Exception("Device is already assigned.");
        }

        device.AssignedUserId = userIdentifier;
        device.AssignedUserEmail = userEmail;
        await _devicesRepository.UpdateDeviceAsync(device);
    }

    public async Task UnassignDeviceAsync(string deviceIdentifier, string userIdentifier)
    {
        var device = await _devicesRepository.RetrieveDeviceByIdAsync(deviceIdentifier);
        
        if (device.AssignedUserId != userIdentifier)
        {
            throw new Exception("You are not authorized to unassign this device.");
        }

        device.AssignedUserId = string.Empty;
        device.AssignedUserEmail = string.Empty;
        await _devicesRepository.UpdateDeviceAsync(device);
    }
}
