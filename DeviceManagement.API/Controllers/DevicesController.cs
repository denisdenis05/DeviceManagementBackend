using Microsoft.AspNetCore.Mvc;
using DeviceManagement.Data.Models.Devices;
using DeviceManagement.Business.Services.Devices;
using DeviceManagement.API.CONSTANTS;
using DeviceManagement.API.Requests.Devices;

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
    public async Task<IActionResult> RetrieveAllDevicesAsync()
    {
        var devices = await _devicesService.RetrieveAllDevicesAsync();
        return Ok(devices);
    }

    [HttpGet(ApiConstants.GetDeviceEndpoint)]
    public async Task<IActionResult> GetRequest([FromQuery] string identifier)
    {
        try
        {
            var device = await _devicesService.RetrieveDeviceByIdAsync(identifier);
            return Ok(device);
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPost(ApiConstants.AddEndpoint)]
    public async Task<IActionResult> AddRequest([FromBody] AddDeviceRequest request)
    {
        try
        {
            await _devicesService.AddDeviceAsync(request.ToAddDeviceDto());
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPost(ApiConstants.EditEndpoint)]
    public async Task<IActionResult> EditRequest([FromBody] EditDeviceRequest request)
    {
        try
        {
            await _devicesService.EditDeviceAsync(request.ToEditDeviceDto());
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpDelete(ApiConstants.DeleteEndpoint)]
    public async Task<IActionResult> DeleteRequest([FromQuery] string identifier)
    {
        try
        {
            await _devicesService.DeleteDeviceAsync(identifier);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
}
