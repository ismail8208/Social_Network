namespace MediaLink.Domain.Events.EndorsementEvents;

public class EndorsementCreatedEvent : BaseEvent
{
    public EndorsementCreatedEvent(Endorsement endorsement )
    {
        Endorsement = endorsement;
    }
    public Endorsement Endorsement { get; }
}
