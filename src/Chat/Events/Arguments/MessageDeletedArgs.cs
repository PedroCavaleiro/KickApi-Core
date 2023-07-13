using KickStreaming.Chat.Events.Models;

namespace KickStreaming.Chat.Events.Arguments; 

public class MessageDeletedArgs : EventArgs {

    public DeletedMessage? Message { get; set; }

    public MessageDeletedArgs(DeletedMessage? args) => Message = args;

}