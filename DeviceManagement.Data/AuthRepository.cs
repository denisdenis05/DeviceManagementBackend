using DeviceManagement.Data.CONSTANTS;
using DeviceManagement.Data.Models.Auth;
using MongoDB.Driver;

namespace DeviceManagement.Data;

public class AuthRepository : IAuthRepository
{
    private readonly IMongoCollection<UserDto> _usersCollection;

    public AuthRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(DatabaseConstants.DatabaseName);
        _usersCollection = database.GetCollection<UserDto>(DatabaseConstants.UsersCollectionName);
    }

    public async Task InsertUserAsync(UserDto userToInsert)
    {
        await _usersCollection.InsertOneAsync(userToInsert);
    }

    public async Task<UserDto?> RetrieveUserByEmailAsync(string emailAddress)
    {
        var filter = Builders<UserDto>.Filter.Eq(user => user.Email, emailAddress);
        return await _usersCollection.Find(filter).FirstOrDefaultAsync();
    }
}
