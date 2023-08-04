namespace MediaLink.Domain.Events.LikeEvents;

public class LikeCreatedEvent : BaseEvent
{
    public LikeCreatedEvent(Like like)
    {
        Like = like;
    }
   
    public Like Like { get; }
}
