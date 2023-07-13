using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Models; 

public class BanChatterData {
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("chat_id")]
    public int ChatId { get; set; }
    [JsonPropertyName("banned_id")]
    public int BannedId { get; set; }
    [JsonPropertyName("banner_id")]
    public int BannerId { get; set; }
    [JsonPropertyName("reason")]
    public string reason { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("permanent")]
    public bool Permanent { get; set; }
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("expires_at")]
    public DateTime ExpiresAt { get; set; }
}