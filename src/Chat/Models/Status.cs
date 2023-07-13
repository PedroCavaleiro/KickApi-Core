using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Models; 

public class Status {
    [JsonPropertyName("error")]
    public bool Error { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}