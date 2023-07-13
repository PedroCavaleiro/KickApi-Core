using KickStreaming.Chat.Models;
using Extensions;
using KickStreaming.Chat.Events;
using KickStreaming.Chat.Events.Arguments;
using KickStreaming.Chat.Events.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharper;
using JsonSerializer = System.Text.Json.JsonSerializer;
#pragma warning disable CS8602

// ReSharper disable EmptyGeneralCatchClause

namespace KickStreaming.Chat;

public class KickChat {

    private const string PusherUrl =
        "wss://ws-us2.pusher.com/app/eb1d5f283081a78b932c?protocol=7&client=js&version=7.6.0&flash=false";
    
    private readonly KickApi   _kickApi;
    private readonly WebSocket _pusher;

    public int ChatroomId;
    public int ChannelId;

    private char _commandTrigger = '\'';
    
    #region "Events"
    public event ConnectedHandler?        PusherConnected;
    public event PusherSubscribedHandler? SubscribedToChannel;
    // chatroom.{id}.v2
    public event MessageDeletedHandler?     MessageDeleted;
    public event MessageReceivedHandler?    MessageReceived;
    public event ChatClearedHandler?        ChatCleared;
    public event SubscribedHandler?         Subscribed;
    public event GiftedSubscriptionHandler? GiftedSubscriptions;
    public event CommandReceivedHandler?    CommandReceived;
    // channel.{id}
    public event StreamStatusChangeHandler? StreamStatusChanged;
    
    #endregion

    internal KickChat(KickApi kickApi) {
        _kickApi = kickApi;
        
        _pusher = new WebSocket(
            LoggerFactory.Create(b => b.AddConsole()).CreateLogger<WebSocket>(), 
            PusherUrl,
            false
        );
    }

    /// <summary>
    /// Send a message to a specific Kick chatroom.
    /// Messages to kick chats can only be sent through REST API 
    /// </summary>
    /// <param name="message">Message contents</param>
    /// <param name="chatroomId">Chatroom id to send the message to</param>
    /// <param name="token">Authentication token, if not passed it will use the global token</param>
    /// <returns>The ID of the message and chatroom and the <see cref="Models.SentMessage">message object</see> inside Data</returns>
    public async Task<SendMessageResponse?> SendMessage(string message, string chatroomId, string token = "") {

        var request = new Dictionary<string, object> {
            { "content", message },
            { "type", "message" }
        };

        var sendMessageRequest = await _kickApi.CycleTls.SendAsync(
            KickApi.PostSpoofOptions(
                $"messages/send/{chatroomId}",
                request, 
                new Dictionary<string, string> {
                    { "Accept", "application/json" },
                    { "Content-Type", "application/json" },
                    { "Authorization", $"Bearer {(token.IsNullOrEmpty() ? _kickApi.Token : token)}"},
                    { "x-xsrf-token", token.IsNullOrEmpty() ? _kickApi.Token : token}
                }
            )
        );

        return JsonSerializer.Deserialize<SendMessageResponse>(sendMessageRequest.Body);
    }

    /// <summary>
    /// Replies to a message
    /// </summary>
    /// <param name="message">Message to be sent</param>
    /// <param name="chatroomId">Chatroom id to send the message to</param>
    /// <param name="metadata">Metadata of the original message <see cref="ReplyMetadata">ReplyMetadata</see></param>
    /// <param name="token">Authentication token, if not passed it will use the global token</param>
    /// <returns>The ID of the message and chatroom and the <see cref="Models.SentMessage">message object</see> inside Data</returns>
    public async Task<SendMessageResponse?> ReplyToMessage(string message, int chatroomId, ReplyMetadata metadata, string token = "") {
        var request = new Dictionary<string, object> {
            { "content", message },
            { "metadata", metadata },
            { "type", "reply" }
        };
        
        var sendMessageRequest = await _kickApi.CycleTls.SendAsync(
            KickApi.PostSpoofOptions(
                $"messages/send/{chatroomId}",
                request, 
                new Dictionary<string, string> {
                    { "Accept", "application/json" },
                    { "Content-Type", "application/json" },
                    { "Authorization", $"Bearer {(token.IsNullOrEmpty() ? _kickApi.Token : token)}"},
                    { "x-xsrf-token", token.IsNullOrEmpty() ? _kickApi.Token : token}
                }
            )
        );

        return JsonSerializer.Deserialize<SendMessageResponse>(sendMessageRequest.Body);
    }

    /// <summary>
    /// Deletes a message from the chat
    /// The user requires moderation permissions, it can be the streamer
    /// </summary>
    /// <param name="chatroomId">Chatroom id where the message is</param>
    /// <param name="messageId">Message id to delete</param>
    /// <param name="token">Authentication token, if not passed it will use the global token</param>
    public async Task DeleteMessage(int chatroomId, string messageId, string token = "") {
        await _kickApi.CycleTls.SendAsync(
            KickApi.GetSpoofOptions(
                $"chatrooms/{chatroomId}/messages/{messageId}",
                HttpMethod.Delete,
                new Dictionary<string, string> {
                    { "Accept", "application/json" },
                    { "Authorization", $"Bearer {(token.IsNullOrEmpty() ? _kickApi.Token : token)}"},
                    { "x-xsrf-token", token.IsNullOrEmpty() ? _kickApi.Token : token}
                }
            )
        );
    }

    /// <summary>
    /// Gets the chatter data
    /// </summary>
    /// <param name="streamer">Streamer on witch the chatter is talking</param>
    /// <param name="chatter">Name of the chatter</param>
    /// <param name="token">Authentication token, if not passed it will use the global token</param>
    /// <returns>The chatter information. <see cref="Chatter"/></returns>
    public async Task<Chatter?> GetChatter(string streamer, string chatter, string token = "") {
        var response = await _kickApi.CycleTls.SendAsync(
            KickApi.GetSpoofOptions(
                $"channels/{streamer}/users/{chatter}",
                2,
                HttpMethod.Get,
                new Dictionary<string, string> {
                    { "Accept", "application/json" },
                    { "Authorization", $"Bearer {(token.IsNullOrEmpty() ? _kickApi.Token : token)}"},
                    { "x-xsrf-token", token.IsNullOrEmpty() ? _kickApi.Token : token}
                }
            )
        );
        return response.Status == 404 ? null : JsonSerializer.Deserialize<Chatter>(response.Body);
    }

    /// <summary>
    /// Puts the chatter in a timeout
    /// </summary>
    /// <param name="streamer">Streamer name</param>
    /// <param name="usernameToBan">Username to ban, not user id</param>
    /// <param name="reason">Reason for the timeout</param>
    /// <param name="duration">Duration of the timeout in seconds</param>
    /// <param name="token">Authentication token, if not passed it will use the global token</param>
    public async Task<BanChatterResponse?> TimeoutChatter(string streamer, string usernameToBan, string reason = "", int duration = 30, string token = "") {
        var req = new Dictionary<string, object> {
            { "banned_username", usernameToBan },
            { "permanent", false },
            { "reason", reason },
            { "duration", duration }
        };
        var response = await _kickApi.CycleTls.SendAsync(
            KickApi.PostSpoofOptions(
                $"channels/{streamer}/bans",
                req,
                new Dictionary<string, string> {
                    { "Accept", "application/json" },
                    { "Authorization", $"Bearer {(token.IsNullOrEmpty() ? _kickApi.Token : token)}"},
                    { "x-xsrf-token", token.IsNullOrEmpty() ? _kickApi.Token : token}
                }
            )
        );
        return response.Status == 404 ? null : JsonSerializer.Deserialize<BanChatterResponse?>(response.Body);
    }
    
    /// <summary>
    /// Bans a chatter
    /// </summary>
    /// <param name="streamer">Streamer name</param>
    /// <param name="usernameToBan">Username to ban, not user id</param>
    /// <param name="reason">Reason for the timeout</param>
    /// <param name="token">Authentication token, if not passed it will use the global token</param>
    public async Task<BanChatterResponse?> BanChatter(string streamer, string usernameToBan, string reason = "", string token = "") {
        var req = new Dictionary<string, object> {
            { "banned_username", usernameToBan },
            { "permanent", true },
            { "reason", reason }
        };
        var response = await _kickApi.CycleTls.SendAsync(
            KickApi.PostSpoofOptions(
                $"channels/{streamer}/bans",
                req,
                new Dictionary<string, string> {
                    { "Accept", "application/json" },
                    { "Authorization", $"Bearer {(token.IsNullOrEmpty() ? _kickApi.Token : token)}"},
                    { "x-xsrf-token", token.IsNullOrEmpty() ? _kickApi.Token : token}
                }
            )
        );
        return response.Status == 404 ? null : JsonSerializer.Deserialize<BanChatterResponse?>(response.Body);
    }
    
    /// <summary>
    /// Unbans a chatter from a channel
    /// </summary>
    /// <param name="streamer">Streamer name</param>
    /// <param name="usernameToUnban">Username to ban, not user id</param>
    /// <param name="token">Authentication token, if not passed it will use the global token</param>
    public async Task UnbanChatter(string streamer, string usernameToUnban, string token = "") {
        await _kickApi.CycleTls.SendAsync(
            KickApi.GetSpoofOptions(
                $"channels/{streamer}/bans/{usernameToUnban}",
                HttpMethod.Delete,
                new Dictionary<string, string> {
                    { "Accept", "application/json" },
                    { "Authorization", $"Bearer {(token.IsNullOrEmpty() ? _kickApi.Token : token)}"},
                    { "x-xsrf-token", token.IsNullOrEmpty() ? _kickApi.Token : token}
                }
            )
        );
    }

    /// <summary>
    /// Starts listening to realtime events
    /// </summary>
    /// <param name="chatroomId">Chatroom to listen to</param>
    /// <param name="channelId">Channel to listen to</param>
    /// <param name="commandTrigger">The trigger for command by default is '</param>
    public void ListenFor(int chatroomId, int channelId, char commandTrigger = '\'') {

        _commandTrigger = commandTrigger;

        ChannelId  = channelId;
        ChatroomId = chatroomId;
        
        _pusher.OnMessage += (_, ea) => {
            try {
                Console.WriteLine(ea.Data);
                var msg    = JsonSerializer.Deserialize<PusherMessage>(ea.Data);
                try {
                    var @event = msg.Event.ParseEnum<PusherMessage.PusherEvents>();
                    // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                    switch (@event) {
                        case PusherMessage.PusherEvents.ConnectionEstablished:
                            PusherConnected(ea, new ConnectedArgs(JsonSerializer.Deserialize<ConnectedMessage>(msg.Data)));
                            break;
                        case PusherMessage.PusherEvents.SubscriptionSucceeded:
                            SubscribedToChannel(ea, new PusherSubscribedArgs(msg.Channel));
                            break;
                        case PusherMessage.PusherEvents.ChatMessageEvent:
                            var receivedMessage = JsonSerializer.Deserialize<ReceivedMessage>(msg.Data);
                            if(receivedMessage.Content[0] == _commandTrigger)
                                CommandReceived(ea, new CommandReceivedArgs(receivedMessage, this, _commandTrigger));
                            else
                                MessageReceived(ea, new MessageReceivedArgs(receivedMessage, this));
                            break;
                        case PusherMessage.PusherEvents.MessageDeletedEvent:
                            MessageDeleted(ea, new MessageDeletedArgs(JsonSerializer.Deserialize<DeletedMessage>(msg.Data)));
                            break;
                        case PusherMessage.PusherEvents.ChatroomClearEvent:
                            ChatCleared(ea, new ChatClearedArgs(JsonSerializer.Deserialize<GenericEventData>(msg.Data)));
                            break;
                        case PusherMessage.PusherEvents.ChannelSubscriptionEvent: break;               // channel
                        case PusherMessage.PusherEvents.LuckyUsersWhoGotGiftSubscriptionsEvent: break; //channel
                        case PusherMessage.PusherEvents.GiftsLeaderboardUpdated: break;                //channel
                        case PusherMessage.PusherEvents.StreamerIsLive:
                            StreamStatusChanged(ea, true);
                            break;
                        case PusherMessage.PusherEvents.StopStreamBroadcast:
                            StreamStatusChanged(ea, false);
                            break;
                        case PusherMessage.PusherEvents.StreamHostEvent: break;                 // chat -> has handler
                        case PusherMessage.PusherEvents.ChatMoveToSupportedChannelEvent: break; // channel -> has handler
                        case PusherMessage.PusherEvents.GiftedSubscriptionsEvent:
                            var giftedSubscriptionEventData = JObject.Parse(msg.Data);
                            var gifter                      = (string)giftedSubscriptionEventData["gifter_username"];
                            var giftedUsernamesToken        = giftedSubscriptionEventData.GetValue("gifter_username");
                            var giftedUsers =
                                JsonConvert.DeserializeObject<List<string>>(giftedUsernamesToken.ToString());
                            GiftedSubscriptions(ea, new GiftedSubscriptionsArgs(gifter, giftedUsers));
                            break;
                        case PusherMessage.PusherEvents.SubscriptionEvent:
                            var subscriptionEventData = JObject.Parse(msg.Data);
                            Subscribed(ea, 
                                       new SubscribedArgs(
                                            (string)subscriptionEventData["username"],
                                            (int)subscriptionEventData["months"]
                                       )
                            );
                            break;
                        case PusherMessage.PusherEvents.FollowersUpdated: break; //channel
                    }
                } catch (Exception) { }
            } catch (Exception) { }
        };

        _pusher.OnError += (_, _) => {
            _pusher.Close();
            _kickApi.Logger.Log<KickChat>("An error occurred on the pusher socket", Logger.Level.Error);
        };

        _pusher.OnOpen += (_, _) => {
            _pusher.SendAsync(JsonSerializer.Serialize(new
            {
                @event = "pusher:subscribe",
                data   = new {
                    auth    = "",
                    channel = $"chatrooms.{chatroomId}.v2"
                }
            }), _ => {
                _kickApi.Logger.Log<KickChat>($"Subscribed to \"chatrooms.{chatroomId}.v2\"", Logger.Level.Info);
            });
            
            _pusher.SendAsync(JsonSerializer.Serialize(new
            {
                @event = "pusher:subscribe",
                data = new {
                    auth    = "",
                    channel = $"channel.{channelId}"
                }
            }), _ => {
                _kickApi.Logger.Log<KickChat>($"Subscribed to \"channel.{channelId}\"", Logger.Level.Info);
            });
        };

        _pusher.Connect();
        
    }

    /// <summary>
    /// Stops listening for realtime events
    /// </summary>
    public async Task StopListening() {
        await _pusher.SendTaskAsync(JsonSerializer.Serialize(new
        {
            @event = "pusher:unsubscribe",
            data = new {
                auth    = "",
                channel = $"channel.{ChannelId}"
            }
        }));
        _kickApi.Logger.Log<KickChat>($"Unsubscribed from \"channel.{ChannelId}\"", Logger.Level.Info);
        await _pusher.SendTaskAsync(JsonSerializer.Serialize(new {
            @event = "pusher:unsubscribe",
            data = new {
                auth    = "",
                channel = $"chatrooms.{ChatroomId}.v2"
            }
        }));
        _kickApi.Logger.Log<KickChat>($"Unsubscribed from \"chatrooms.{ChatroomId}.v2\"", Logger.Level.Info);
        await _pusher.CloseTaskAsync(CloseStatusCode.Normal, "");
    }

}