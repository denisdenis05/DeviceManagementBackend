using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeviceManagement.Data.Models.Auth;

[BsonIgnoreExtraElements]
public class UserDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("Email")]
    public string Email { get; set; } = null!;

    [BsonElement("PasswordHash")]
    public string PasswordHash { get; set; } = null!;
}
