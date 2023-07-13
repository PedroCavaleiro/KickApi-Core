using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Events.Models; 

public class DeletedMessage {
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("message")]
    public DeletedMessageInfo Message { get; set; }
}

public class DeletedMessageInfo
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}