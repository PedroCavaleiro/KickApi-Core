using System.Text.Json.Serialization;

namespace KickStreaming.User.Models; 

public class Category {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("icon")]
    public string Icon { get; set; }
}