using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DeviceManagement.Business.Models.AI;

public class LmStudioChatResponse
{
    [JsonPropertyName("output")]
    public List<LmStudioOutput>? Output { get; set; }

    [JsonPropertyName("choices")]
    public List<LmStudioChoice>? Choices { get; set; }
}
