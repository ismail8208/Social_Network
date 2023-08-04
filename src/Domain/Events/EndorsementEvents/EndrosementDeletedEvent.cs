namespace MediaLink.Domain.Events.EndorsementEvents;

public class EndrosementDeletedEvent : BaseEvent
{
    public EndrosementDeletedEvent(Endorsement endorsement)
    {
        Endorsement = endorsement;
    }
    public Endorsement Endorsement { get; }
}
