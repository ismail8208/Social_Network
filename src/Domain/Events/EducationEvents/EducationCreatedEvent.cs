namespace MediaLink.Domain.Events.EducationEvents;

public class EducationCreatedEvent : BaseEvent
{
    public EducationCreatedEvent(Education education)
    {
        Education = education;
    }
    public Education Education { get; }
}
