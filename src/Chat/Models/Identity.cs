using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Models; 

public class Identity {
    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("badges")]
    public List<Badge> Badges { get; set; }
}