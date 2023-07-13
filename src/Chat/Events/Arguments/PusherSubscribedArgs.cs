namespace KickStreaming.Chat.Events.Arguments; 

public class PusherSubscribedArgs : EventArgs{
    public string? Channel { get; set; }

    public PusherSubscribedArgs(string? args) => Channel = args;
}