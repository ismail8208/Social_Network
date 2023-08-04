
namespace MediaLink.Domain.Events.SkillEvents;

public class SkillCreatedEvent : BaseEvent
{
    public SkillCreatedEvent(Skill skill )
    {
        Skill = skill;
    }

    public Skill Skill { get; }
}
