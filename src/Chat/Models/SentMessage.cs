using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Models; 

public class SentMessage {
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("chatroom_id")]
    public int ChatroomId { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("sender")]
    public Sender Sender { get; set; }
    
    [JsonPropertyName("metadata")]
    public ReplyMetadata Metadata { get; set; }
}