using DeviceManagement.API.Controllers;
using DeviceManagement.API.Requests.AI;
using DeviceManagement.Business.Services.AI;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DeviceManagement.Tests;

public class AiControllerTests
{
    private readonly Mock<IAiService> _aiServiceMock;
    private readonly AiController _aiController;

    public AiControllerTests()
    {
        _aiServiceMock = new Mock<IAiService>();
        _aiController = new AiController(_aiServiceMock.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Chat_WhenMessageIsIsNullOrWhitespace_ShouldReturnBadRequest(string? invalidMessage)
    {
        var chatRequest = new AiChatRequest { Message = invalidMessage! };

        var actionResult = await _aiController.Chat(chatRequest);

        actionResult.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("Message cannot be empty.");
    }

    [Fact]
    public async Task Chat_WhenAiServiceSucceeds_ShouldReturnOkWithResponse()
    {
        const string userMessage = "Hello AI";
        const string expectedAiResponse = "Hello Human";
        var chatRequest = new AiChatRequest { Message = userMessage };
        _aiServiceMock.Setup(service => service.ChatWithRagAsync(userMessage))
            .ReturnsAsync(expectedAiResponse);

        var actionResult = await _aiController.Chat(chatRequest);

        actionResult.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(new { response = expectedAiResponse });
    }

    [Fact]
    public async Task Chat_WhenAiServiceThrowsException_ShouldReturnInternalServerErrorWithErrorMessage()
    {
        const string userMessage = "Hello AI";
        const string exceptionMessage = "AI is offline";
        var chatRequest = new AiChatRequest { Message = userMessage };
        _aiServiceMock.Setup(service => service.ChatWithRagAsync(userMessage))
            .ThrowsAsync(new Exception(exceptionMessage));

        var actionResult = await _aiController.Chat(chatRequest);

        actionResult.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(500);
        var objectResult = actionResult as ObjectResult;
        objectResult!.Value.Should().Be($"An error occurred while processing the chat: {exceptionMessage}");
    }
}
