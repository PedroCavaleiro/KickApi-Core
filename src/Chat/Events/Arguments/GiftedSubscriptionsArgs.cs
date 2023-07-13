namespace KickStreaming.Chat.Events.Arguments; 

public class GiftedSubscriptionsArgs : EventArgs {
    public string?       Gifter   { get; set; }
    public List<string>? GiftedTo { get; set; }

    public GiftedSubscriptionsArgs(string? gifter, List<string>? giftedTo) {
        Gifter   = gifter;
        GiftedTo = giftedTo;
    }
}