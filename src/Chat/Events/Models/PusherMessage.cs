using System.ComponentModel;
using System.Text.Json.Serialization;

namespace KickStreaming.Chat.Events.Models; 

public class PusherMessage {
    
    [JsonPropertyName("event")]
    public string Event { get; set; }

    [JsonPropertyName("data")]
    public string Data { get; set; }
    
    [JsonPropertyName("channel")]
    public string Channel { get; set; }
    
    public enum PusherEvents {
        [Description("pusher:connection_established")]
        ConnectionEstablished,
        [Description("pusher_internal:subscription_succeeded")]
        SubscriptionSucceeded,
        [Description("App\\Events\\ChatMessageEvent")]
        ChatMessageEvent,
        [Description("App\\Events\\MessageDeletedEvent")]
        MessageDeletedEvent,
        [Description("App\\Events\\ChatroomClearEvent")]
        ChatroomClearEvent,
        [Description("App\\Events\\ChannelSubscriptionEvent")]
        ChannelSubscriptionEvent,
        [Description("App\\Events\\LuckyUsersWhoGotGiftSubscriptionsEvent")]
        LuckyUsersWhoGotGiftSubscriptionsEvent,
        [Description("App\\Events\\GiftsLeaderboardUpdated")]
        GiftsLeaderboardUpdated,
        [Description("App\\Events\\StreamerIsLive")]
        StreamerIsLive,
        [Description("App\\Events\\StopStreamBroadcast")]
        StopStreamBroadcast,
        [Description("App\\Events\\StreamHostEvent")]
        StreamHostEvent,
        [Description("App\\Events\\ChatMoveToSupportedChannelEvent")]
        ChatMoveToSupportedChannelEvent,
        [Description("App\\Events\\GiftedSubscriptionsEvent")]
        GiftedSubscriptionsEvent,
        [Description("App\\Events\\SubscriptionEvent")]
        SubscriptionEvent,
        [Description("App\\Events\\FollowersUpdated")]
        FollowersUpdated
    }
    
}