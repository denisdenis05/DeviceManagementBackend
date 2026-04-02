using DeviceManagement.Business.CONSTANTS;
using DeviceManagement.Data;
using DeviceManagement.Data.Models.Devices;

namespace DeviceManagement.Business.Services.Search;

public class SearchService : ISearchService
{
    private readonly IDevicesRepository _devicesRepository;

    public SearchService(IDevicesRepository devicesRepository)
    {
        _devicesRepository = devicesRepository;
    }

    public async Task<IEnumerable<DeviceDto>> SearchDevicesAsync(string searchQuery)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            return await _devicesRepository.RetrieveAllDevicesAsync();
        }

        var tokens = searchQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var allDevices = await _devicesRepository.RetrieveAllDevicesAsync();

        var rankedDevices = allDevices
            .Select(device => new { Device = device, Score = CalculateRelevanceScore(device, tokens) })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Select(x => x.Device);

        return rankedDevices;
    }

    private int CalculateRelevanceScore(DeviceDto device, string[] tokens)
    {
        var score = 0;

        foreach (var token in tokens)
        {
            if (device.Name.Contains(token, StringComparison.OrdinalIgnoreCase))
            {
                score += SearchConstants.NameWeight;
            }

            if (device.Manufacturer.Contains(token, StringComparison.OrdinalIgnoreCase))
            {
                score += SearchConstants.ManufacturerWeight;
            }

            if (device.Processor.Contains(token, StringComparison.OrdinalIgnoreCase))
            {
                score += SearchConstants.ProcessorWeight;
            }

            if (device.RamAmount.ToString().Contains(token, StringComparison.OrdinalIgnoreCase))
            {
                score += SearchConstants.RamWeight;
            }
        }

        return score;
    }
}
