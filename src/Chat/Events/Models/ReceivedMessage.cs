using System.Text.Json.Serialization;
using KickStreaming.Chat.Models;

namespace KickStreaming.Chat.Events.Models; 

public class ReceivedMessage {
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("chatroom_id")]
    public int ChatroomId { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("sender")]
    public Sender Sender { get; set; }
    
    [JsonPropertyName("metadata")]
    public ReplyMetadata Metadata { get; set; }

    [JsonIgnore] public KickChat Chat { get; set; }

    /// <summary>
    /// Replies to this message
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="token">Authentication token, if not passed it will use the global token</param>
    /// <returns>The ID of the message and chatroom and the <see cref="SentMessage">message object</see> inside Data</returns>
    public async Task<SendMessageResponse?> Reply(string message, string token = "") {
        var metadata = new ReplyMetadata {
            OriginalMessage = new ReplyOriginalMessage {
                Id = Id,
                Content = Content
            },
            OriginalSender = new ReplyOriginalSender {
                Id = Sender.Id.ToString(),
                Username = Sender.Username
            }
        };
       return await Chat.ReplyToMessage(message, ChatroomId, metadata, token);
    }
    
    /// <summary>
    /// Deletes this message
    /// </summary>
    /// <param name="token">Authentication token, if not passed it will use the global token</param>
    public async Task Delete(string token = "") {
        await Chat.DeleteMessage(ChatroomId, Id, token);
    }
}