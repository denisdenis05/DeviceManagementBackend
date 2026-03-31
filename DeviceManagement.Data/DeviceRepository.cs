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

    public async Task<DeviceDto> RetrieveDeviceByIdAsync(string identifier)
    {
        var cursor = await _devicesCollection.FindAsync(d => d.Id == identifier);
        return await cursor.FirstOrDefaultAsync();
    }

    public async Task InsertDeviceAsync(DeviceDto device)
    {
        await _devicesCollection.InsertOneAsync(device);
    }

    public async Task UpdateDeviceAsync(DeviceDto device)
    {
        await _devicesCollection.ReplaceOneAsync(d => d.Id == device.Id, device);
    }

    public async Task DeleteDeviceAsync(string identifier)
    {
        await _devicesCollection.DeleteOneAsync(d => d.Id == identifier);
    }
}
