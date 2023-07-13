using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Models; 

public class Chatter {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("profile_pic")]
    public string ProfilePic { get; set; }

    [JsonPropertyName("is_staff")]
    public bool IsStaff { get; set; }

    [JsonPropertyName("is_channel_owner")]
    public bool IsChannelOwner { get; set; }

    [JsonPropertyName("is_moderator")]
    public bool IsModerator { get; set; }

    [JsonPropertyName("badges")]
    public List<Badge> Badges { get; set; }

    [JsonPropertyName("following_since")]
    public DateTime FollowingSince { get; set; }

    [JsonPropertyName("subscribed_for")]
    public int SubscribedFor { get; set; }

    [JsonPropertyName("banned")]
    public object Banned { get; set; }
}