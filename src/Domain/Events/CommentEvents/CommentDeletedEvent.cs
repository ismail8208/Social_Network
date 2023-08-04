namespace MediaLink.Domain.Events.CommentEvents;

public class CommentDeletedEvent : BaseEvent
{
    public CommentDeletedEvent(Comment comment)
    {
        Comment = comment;
    }
   
    public Comment Comment { get; }
}
