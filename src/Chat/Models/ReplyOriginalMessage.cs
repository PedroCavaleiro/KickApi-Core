using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Models; 

public class ReplyOriginalMessage {
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}