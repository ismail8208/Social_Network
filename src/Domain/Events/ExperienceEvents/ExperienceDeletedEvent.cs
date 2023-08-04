namespace MediaLink.Domain.Events.ExperienceEvents;

public class ExperienceDeletedEvent : BaseEvent
{
    public ExperienceDeletedEvent(Experience experience)
    {
        Experience =    experience;
    }

    public Experience Experience { get; }
}
