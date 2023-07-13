using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Models; 

public class UnbanChatterResponse {
    [JsonPropertyName("status")]
    public bool Status { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
}