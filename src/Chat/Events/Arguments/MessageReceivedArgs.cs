using KickStreaming.Chat.Events.Models;

namespace KickStreaming.Chat.Events.Arguments; 

public class MessageReceivedArgs : EventArgs {

    public ReceivedMessage? Message { get; set; }

    public MessageReceivedArgs(ReceivedMessage? args, KickChat chatInstance) {
        Message      = args;
        Message.Chat = chatInstance;
    }

}