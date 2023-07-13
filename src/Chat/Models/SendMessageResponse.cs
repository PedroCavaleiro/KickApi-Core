using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Models; 

public class SendMessageResponse {
    [JsonPropertyName("status")]
    public Status Status { get; set; }

    [JsonPropertyName("data")]
    public SentMessage Data { get; set; }
}