using KickStreaming.Chat.Events.Models;

namespace KickStreaming.Chat.Events.Arguments; 

public class ConnectedArgs : EventArgs {
    public ConnectedMessage? Message { get; set; }

    public ConnectedArgs(ConnectedMessage? args) => Message = args;
}