using System.Text.Json.Serialization;

namespace KickStreaming.User.Models; 

public class RecentCategory {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("category_id")]
    public int CategoryId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; }

    [JsonPropertyName("description")]
    public object Description { get; set; }

    [JsonPropertyName("deleted_at")]
    public object DeletedAt { get; set; }

    [JsonPropertyName("viewers")]
    public int Viewers { get; set; }

    [JsonPropertyName("banner")]
    public Banner Banner { get; set; }

    [JsonPropertyName("category")]
    public Category Category { get; set; }
}