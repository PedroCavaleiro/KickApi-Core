namespace KickStreaming.Chat.Events.Arguments; 

public class SubscribedArgs : EventArgs {
    public string? Username { get; set; }
    public int? Months { get; set; }

    public SubscribedArgs(string? username, int? months) {
        Username = username;
        Months   = months;
    }
}