using DeviceManagement.API.Requests.AI;
using DeviceManagement.Business.Services.AI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AiController : ControllerBase
{
    private readonly IAiService _aiService;

    public AiController(IAiService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] AiChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest("Message cannot be empty.");
        }

        try
        {
            var response = await _aiService.ChatWithRagAsync(request.Message);
            return Ok(new { response });
        }
        catch (Exception exception)
        {
            return StatusCode(500, $"An error occurred while processing the chat: {exception.Message}");
        }
    }
}
