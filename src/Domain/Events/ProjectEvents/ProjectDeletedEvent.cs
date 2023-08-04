namespace MediaLink.Domain.Events.ProjectEvents;

public class ProjectDeletedEvent : BaseEvent
{
    public ProjectDeletedEvent(Project project)
    {
        Project = project;
    }

    public Project Project { get; set; }
}