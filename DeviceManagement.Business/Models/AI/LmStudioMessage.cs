using System.Text.Json.Serialization;

namespace DeviceManagement.Business.Models.AI;

public class LmStudioMessage
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}
