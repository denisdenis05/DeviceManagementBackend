using MongoDB.Driver;
using DeviceManagement.Data.Models.Devices;
using DeviceManagement.Data.CONSTANTS;

namespace DeviceManagement.Data;

public class DeviceRepository : IDevicesRepository
{
    private readonly IMongoCollection<DeviceDto> _devicesCollection;

    public DeviceRepository(IMongoClient mongoClient)
    {
        var mongoDatabase = mongoClient.GetDatabase(DatabaseConstants.DatabaseName);
        _devicesCollection = mongoDatabase.GetCollection<DeviceDto>(DatabaseConstants.DevicesCollectionName);
    }

    public async Task<IEnumerable<DeviceDto>> RetrieveAllDevicesAsync()
    {
        var allDevicesCursor = await _devicesCollection.FindAsync(FilterDefinition<DeviceDto>.Empty);
        return await allDevicesCursor.ToListAsync();
    }
}
