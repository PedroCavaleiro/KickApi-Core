using KickStreaming.Chat.Events.Arguments;

namespace KickStreaming.Chat.Events;

public delegate void ConnectedHandler(object          sender, ConnectedArgs            args);
public delegate void PusherSubscribedHandler(object   sender, PusherSubscribedArgs     args);
public delegate void MessageDeletedHandler(object     sender, MessageDeletedArgs       args);
public delegate void CommandReceivedHandler(object    sender, CommandReceivedArgs      args);
public delegate void MessageReceivedHandler(object    sender, MessageReceivedArgs      args);
public delegate void ChatClearedHandler(object        sender, ChatClearedArgs          args);
public delegate void StreamStatusChangeHandler(object sender, bool                     live);
public delegate void SubscribedHandler(object         sender, SubscribedArgs           args);
public delegate void GiftedSubscriptionHandler(object sender,  GiftedSubscriptionsArgs args);