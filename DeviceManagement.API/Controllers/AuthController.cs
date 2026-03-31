using DeviceManagement.API.CONSTANTS;
using DeviceManagement.API.Requests.Auth;
using DeviceManagement.Business.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost(ApiConstants.AuthRegisterEndpoint)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var response = await _authService.RegisterAsync(request.ToRegisterDto());
            return Ok(response);
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPost(ApiConstants.AuthLoginEndpoint)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await _authService.LoginAsync(request.ToLoginDto());
            return Ok(response);
        }
        catch
        {
            return Unauthorized();
        }
    }
}
