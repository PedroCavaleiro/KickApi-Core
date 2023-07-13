using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Events.Models; 

public class GenericEventData {
    [JsonPropertyName("id")]
    public string Id { get; set; }
}