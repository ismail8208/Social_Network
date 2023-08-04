namespace MediaLink.Domain.Events.ExperienceEvents;

public class ExperienceCreatedEvent : BaseEvent
{
    public ExperienceCreatedEvent(Experience experience)
    {
        Experience =    experience;
    }

    public Experience Experience { get; }
}
