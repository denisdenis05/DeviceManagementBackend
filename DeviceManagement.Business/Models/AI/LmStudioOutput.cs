using System.Text.Json.Serialization;

namespace DeviceManagement.Business.Models.AI;

public class LmStudioOutput
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
