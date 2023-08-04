namespace MediaLink.Domain.Events.CommentEvents;

public class CommentCreatedEvent : BaseEvent
{
    public CommentCreatedEvent(Comment comment)
    {
        Comment = comment;
    }

    public Comment Comment { get; }
}