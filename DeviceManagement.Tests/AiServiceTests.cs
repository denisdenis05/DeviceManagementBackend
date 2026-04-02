using DeviceManagement.Business.CONSTANTS;
using DeviceManagement.Business.Models.AI;
using DeviceManagement.Business.Services.AI;
using DeviceManagement.Data;
using DeviceManagement.Data.Models.Devices;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace DeviceManagement.Tests;

public class AiServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IDevicesRepository> _devicesRepositoryMock;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly AiService _aiService;

    public AiServiceTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _configurationMock = new Mock<IConfiguration>();
        _devicesRepositoryMock = new Mock<IDevicesRepository>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        SetupConfiguration();
        SetupHttpClient();

        _aiService = new AiService(
            _httpClientFactoryMock.Object,
            _configurationMock.Object,
            _devicesRepositoryMock.Object);
    }

    [Fact]
    public async Task ChatWithRagAsync_WhenLocalLlmSucceeds_ShouldReturnContentFromChoices()
    {
        const string userMessage = "Tell me about my devices";
        var devices = new List<DeviceDto> { new DeviceDto { Id = "1", Name = "Galaxy" } };
        _devicesRepositoryMock.Setup(repository => repository.RetrieveAllDevicesAsync())
            .ReturnsAsync(devices);
        var expectedResponse = new LmStudioChatResponse
        {
            Choices = new List<LmStudioChoice>
            {
                new LmStudioChoice { Message = new LmStudioMessage { Content = "You have one Galaxy." } }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        var response = await _aiService.ChatWithRagAsync(userMessage);

        response.Should().Be("You have one Galaxy.");
    }

    [Fact]
    public async Task ChatWithRagAsync_WhenLocalLlmReturnsOutputInsteadOfChoices_ShouldReturnContentFromOutput()
    {
        const string userMessage = "Tell me about my devices";
        var devices = new List<DeviceDto> { new DeviceDto { Id = "1", Name = "Galaxy" } };
        _devicesRepositoryMock.Setup(repository => repository.RetrieveAllDevicesAsync())
            .ReturnsAsync(devices);
        var expectedResponse = new LmStudioChatResponse
        {
            Output = new List<LmStudioOutput>
            {
                new LmStudioOutput { Content = "You have one Galaxy." }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        var response = await _aiService.ChatWithRagAsync(userMessage);

        response.Should().Be("You have one Galaxy.");
    }

    [Fact]
    public async Task GenerateDeviceDescriptionAsync_WhenApiKeyMissing_ShouldCallWithoutAuthHeader()
    {
        _configurationMock.Setup(configuration => configuration[AiConstants.AiSettingsApiKey]).Returns(string.Empty);
        var device = new DeviceDto { Name = "Galaxy" };
        var expectedResponse = new LmStudioChatResponse
        {
            Choices = new List<LmStudioChoice>
            {
                new LmStudioChoice { Message = new LmStudioMessage { Content = "Description" } }
            }
        };
        SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

        var response = await _aiService.GenerateDeviceDescriptionAsync(device);

        response.Should().Be("Description");
    }

    [Fact]
    public async Task CallLmStudioAsync_WhenApiReturnsNoContent_ShouldReturnEmptyString()
    {
        _devicesRepositoryMock.Setup(repository => repository.RetrieveAllDevicesAsync())
            .ReturnsAsync(new List<DeviceDto>());
        SetupHttpResponse(HttpStatusCode.OK, new LmStudioChatResponse());

        var response = await _aiService.ChatWithRagAsync("query");

        response.Should().BeEmpty();
    }

    [Fact]
    public async Task CallLmStudioAsync_WhenApiReturnsError_ShouldThrowException()
    {
        _devicesRepositoryMock.Setup(repository => repository.RetrieveAllDevicesAsync())
            .ReturnsAsync(new List<DeviceDto>());
        SetupHttpResponse(HttpStatusCode.InternalServerError, null);

        var action = async () => await _aiService.ChatWithRagAsync("query");

        await action.Should().ThrowAsync<HttpRequestException>();
    }

    private void SetupConfiguration()
    {
        _configurationMock.Setup(configuration => configuration[AiConstants.AiSettingsBaseUrl]).Returns("http://localhost:1234");
        _configurationMock.Setup(configuration => configuration[AiConstants.AiSettingsApiKey]).Returns("testKey");
        _configurationMock.Setup(configuration => configuration[AiConstants.AiSettingsModelName]).Returns("testModel");
    }

    private void SetupHttpClient()
    {
        var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost:1234")
        };
        _httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);
    }

    private void SetupHttpResponse(HttpStatusCode statusCode, object? content)
    {
        var jsonContent = content != null ? JsonSerializer.Serialize(content) : string.Empty;
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(jsonContent)
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
    }
}
