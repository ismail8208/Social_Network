namespace MediaLink.Domain.Events;
public class UserCreatedEvent : BaseEvent
{
    public UserCreatedEvent(InnerUser? innerUser)
    {
        InnerUser = innerUser;
    }

    public InnerUser? InnerUser { get; set; }
}
