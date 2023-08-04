namespace MediaLink.Domain.Events.ShareEvents;

public class ShareDeletedEvent : BaseEvent
{
    public ShareDeletedEvent(Share share)
    {
        Share = share;
    }

    public Share Share { get; }
}
