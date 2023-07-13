using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Models; 

public class ReplyOriginalSender {
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }
}