using System.Text.Json.Serialization;

namespace DeviceManagement.Business.Models.AI;

public class LmStudioChoice
{
    [JsonPropertyName("message")]
    public LmStudioMessage? Message { get; set; }
}
