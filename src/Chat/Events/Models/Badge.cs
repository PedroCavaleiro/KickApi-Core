using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Events.Models; 

public class Badge {
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("count")]
    public int? Count { get; set; }
}