using DeviceManagement.Business.CONSTANTS;
using DeviceManagement.Business.Services.AI;
using DeviceManagement.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DeviceManagement.BackgroundServices;

public class DeviceDescriptionGeneratorService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DeviceDescriptionGeneratorService> _logger;

    public DeviceDescriptionGeneratorService(
        IServiceScopeFactory serviceScopeFactory,
        IConfiguration configuration,
        ILogger<DeviceDescriptionGeneratorService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("hi");
        var intervalSeconds = int.TryParse(
            _configuration[AiConstants.AiSettingsIntervalInSeconds],
            out var parsed) ? parsed : 30;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await GenerateDescriptionsForUnprocessedDevicesAsync(stoppingToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "An error occurred while running the device description generation cycle.");
            }

            await Task.Delay(TimeSpan.FromSeconds(intervalSeconds), stoppingToken);
        }
    }

    private async Task GenerateDescriptionsForUnprocessedDevicesAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var devicesRepository = scope.ServiceProvider.GetRequiredService<IDevicesRepository>();
        var aiService = scope.ServiceProvider.GetRequiredService<IAiService>();

        var allDevices = await devicesRepository.RetrieveAllDevicesAsync();

        var devicesWithoutDescriptions = allDevices.Where(device => string.IsNullOrWhiteSpace(device.Description));
        foreach (var device in devicesWithoutDescriptions)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            try
            {
                var generatedDescription = await aiService.GenerateDeviceDescriptionAsync(device);

                if (!string.IsNullOrWhiteSpace(generatedDescription))
                {
                    device.Description = generatedDescription;
                    await devicesRepository.UpdateDeviceAsync(device);
                    _logger.LogInformation(
                        "Generated description for device with identifier {DeviceId}.",
                        device.Id);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Failed to generate description for device with identifier {DeviceId}.",
                    device.Id);
            }
        }
    }
}
