using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Models; 

public class ReplyMetadata {
    [JsonPropertyName("original_sender")]
    public ReplyOriginalSender OriginalSender { get; set; }

    [JsonPropertyName("original_message")]
    public ReplyOriginalMessage OriginalMessage { get; set; }
}