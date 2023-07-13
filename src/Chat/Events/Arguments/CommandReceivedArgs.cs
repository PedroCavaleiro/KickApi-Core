using KickStreaming.Chat.Events.Models;

namespace KickStreaming.Chat.Events.Arguments; 

public class CommandReceivedArgs {
    public ReceivedMessage? Message           { get; set; }
    public string           Command           { get; set; }
    public string           ArgumentsAsString { get; set; }
    public List<string>     ArgumentsAsList   { get; set; }

    public CommandReceivedArgs(ReceivedMessage? args, KickChat chatInstance, char commandTrigger) {
        Message      = args;
        Message.Chat = chatInstance;

        var data = args.Content.Split(" ").ToList();
        Command = data[0].Remove(0, 1);

        data.RemoveAt(0);
        ArgumentsAsList   = data;
        ArgumentsAsString = string.Join(" ", data);
    }
}