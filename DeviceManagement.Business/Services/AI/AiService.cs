using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DeviceManagement.Business.CONSTANTS;
using DeviceManagement.Business.Models.AI;
using DeviceManagement.Data;
using DeviceManagement.Data.Models.Devices;
using Microsoft.Extensions.Configuration;

namespace DeviceManagement.Business.Services.AI;

public class AiService : IAiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IDevicesRepository _devicesRepository;

    public AiService(
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration,
        IDevicesRepository devicesRepository)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _devicesRepository = devicesRepository;
    }

    public async Task<string> GenerateDeviceDescriptionAsync(DeviceDto device)
    {
        return await CallLmStudioAsync(
            AiConstants.DeviceDescriptionSystemPrompt,
            string.Format(
                AiConstants.DeviceDescriptionUserPromptTemplate,
                device.Name,
                device.Manufacturer,
                device.Type,
                device.OperatingSystem,
                device.OsVersion,
                device.Processor,
                device.RamAmount
            ));
    }

    public async Task<string> ChatWithRagAsync(string userMessage)
    {
        var allDevices = await _devicesRepository.RetrieveAllDevicesAsync();
        var contextBuilder = new StringBuilder();

        foreach (var device in allDevices)
        {
            contextBuilder.AppendLine(string.Format(
                AiConstants.DeviceContextTemplate,
                device.Id,
                device.Name,
                device.Manufacturer,
                device.Type,
                device.OperatingSystem,
                device.OsVersion,
                device.Processor,
                device.RamAmount,
                device.Description
            ));
        }

        var ragUserPrompt = string.Format(
            AiConstants.RagUserPromptTemplate,
            contextBuilder.ToString(),
            userMessage);

        return await CallLmStudioAsync(AiConstants.ChatSystemPrompt, ragUserPrompt);
    }

    private async Task<string> CallLmStudioAsync(string systemPrompt, string userPrompt)
    {
        var baseUrl = _configuration[AiConstants.AiSettingsBaseUrl] ?? string.Empty;
        var apiKey = _configuration[AiConstants.AiSettingsApiKey] ?? string.Empty;
        var modelName = _configuration[AiConstants.AiSettingsModelName] ?? string.Empty;

        var requestPayload = new LmStudioChatRequest
        {
            Model = modelName,
            SystemPrompt = systemPrompt,
            Input = userPrompt
        };

        var jsonContent = JsonSerializer.Serialize(requestPayload);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(baseUrl);

        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);
        }

        var response = await httpClient.PostAsync(AiConstants.ChatCompletionsEndpoint, httpContent);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var chatResponse = JsonSerializer.Deserialize<LmStudioChatResponse>(responseBody);

        var content = chatResponse?.Output?.FirstOrDefault()?.Content 
                      ?? chatResponse?.Choices?.FirstOrDefault()?.Message?.Content 
                      ?? string.Empty;

        return content;
    }
}
