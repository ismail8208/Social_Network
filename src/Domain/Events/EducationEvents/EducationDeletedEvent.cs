namespace MediaLink.Domain.Events.EducationEvents;

public class EducationDeletedEvent :  BaseEvent
{
    public EducationDeletedEvent(Education education)
    {
        Education = education;
    }
    public Education Education { get; }
}
