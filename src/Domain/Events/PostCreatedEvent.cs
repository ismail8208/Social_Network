namespace MediaLink.Domain.Events;
public class PostCreatedEvent : BaseEvent
{
    public PostCreatedEvent(Post post)
    {
        Post= post;
    }
    public Post Post { get; set; }
}
