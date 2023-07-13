using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Models; 

public class Badge {
    
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("text")]
    public string Text { get; set; }
    [JsonPropertyName("active")]
    public bool Active { get; set; }
    [JsonPropertyName("months")]
    public int Months { get; set; }
    
}