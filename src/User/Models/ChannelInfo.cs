using System.Text.Json.Serialization;

namespace KickStreaming.User.Models; 

public class ChannelInfo {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("is_banned")]
    public bool IsBanned { get; set; }

    [JsonPropertyName("playback_url")]
    public string PlaybackUrl { get; set; }

    [JsonPropertyName("vod_enabled")]
    public bool VodEnabled { get; set; }

    [JsonPropertyName("subscription_enabled")]
    public bool SubscriptionEnabled { get; set; }

    [JsonPropertyName("followers_count")]
    public int FollowersCount { get; set; }

    [JsonPropertyName("subscriber_badges")]
    public List<object> SubscriberBadges { get; set; }

    [JsonPropertyName("livestream")]
    public object Livestream { get; set; }

    [JsonPropertyName("role")]
    public object Role { get; set; }

    [JsonPropertyName("muted")]
    public bool Muted { get; set; }

    [JsonPropertyName("follower_badges")]
    public List<object> FollowerBadges { get; set; }

    [JsonPropertyName("offline_banner_image")]
    public object OfflineBannerImage { get; set; }

    [JsonPropertyName("verified")]
    public bool Verified { get; set; }

    [JsonPropertyName("recent_categories")]
    public List<RecentCategory> RecentCategories { get; set; }

    [JsonPropertyName("can_host")]
    public bool CanHost { get; set; }

    [JsonPropertyName("user")]
    public User User { get; set; }

    [JsonPropertyName("chatroom")]
    public Chatroom Chatroom { get; set; }
}