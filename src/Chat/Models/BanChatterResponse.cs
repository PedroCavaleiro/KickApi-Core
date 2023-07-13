using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Models; 

public class BanChatterResponse {
    [JsonPropertyName("status")]
    public Status Status { get; set; }
    [JsonPropertyName("data")]
    public BanChatterData Data { get; set; }
}