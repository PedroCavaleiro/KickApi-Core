using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Events.Models; 

public class ConnectedMessage {
    [JsonPropertyName("socket_id")]
    public string SocketId { get; set; }

    [JsonPropertyName("activity_timeout")]
    public int ActivityTimeout { get; set; }
}