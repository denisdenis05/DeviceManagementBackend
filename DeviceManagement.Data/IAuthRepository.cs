using DeviceManagement.Data.Models.Auth;

namespace DeviceManagement.Data;

public interface IAuthRepository
{
    public Task InsertUserAsync(UserDto userToInsert);
    public Task<UserDto?> RetrieveUserByEmailAsync(string emailAddress);
}
