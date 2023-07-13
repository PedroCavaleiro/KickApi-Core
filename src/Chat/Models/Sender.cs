using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Models; 

public class Sender {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("identity")]
    public Identity Identity { get; set; }
}