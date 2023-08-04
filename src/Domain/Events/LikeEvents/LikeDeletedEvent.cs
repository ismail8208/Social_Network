namespace MediaLink.Domain.Events.LikeEvents;

public class LikeDeletedEvent : BaseEvent
{
    public LikeDeletedEvent(Like like)
    {
        Like = like;   
    }
   
    public Like Like { get; }
}
