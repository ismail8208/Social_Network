namespace MediaLink.Domain.Events.ShareEvents;

public class ShareCreatedEvent : BaseEvent
{
    public ShareCreatedEvent(Share share)
    {
        Share = share;
    }

    public Share Share { get; }    
}
