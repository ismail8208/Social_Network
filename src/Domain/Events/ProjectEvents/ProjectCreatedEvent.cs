namespace MediaLink.Domain.Events.ProjectEvents;

public class ProjectCreatedEvent : BaseEvent
{
    public ProjectCreatedEvent(Project project)
    {
        Project = project;
    }

    public Project Project { get; set; }
}
