using Microsoft.AspNetCore.Mvc;
using DeviceManagement.Data.Models.Devices;
using DeviceManagement.Business.Services.Devices;

namespace DeviceManagement.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IDevicesService _devicesService;

    public DevicesController(IDevicesService devicesService)
    {
        _devicesService = devicesService;
    }

    [HttpGet]
    public async Task<IEnumerable<DeviceDto>> RetrieveAllDevicesAsync()
    {
        return await _devicesService.RetrieveAllDevicesAsync();
    }
}
