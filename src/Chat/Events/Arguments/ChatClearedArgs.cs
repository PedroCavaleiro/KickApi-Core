using KickStreaming.Chat.Events.Models;

namespace KickStreaming.Chat.Events.Arguments;

public class ChatClearedArgs : EventArgs {

    public GenericEventData? Data { get; set; }

    public ChatClearedArgs(GenericEventData? args) => Data = args;

}