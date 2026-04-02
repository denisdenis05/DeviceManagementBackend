using DeviceManagement.Data.Models.Devices;

namespace DeviceManagement.Business.Services.Search;

public interface ISearchService
{
    Task<IEnumerable<DeviceDto>> SearchDevicesAsync(string searchQuery);
}
