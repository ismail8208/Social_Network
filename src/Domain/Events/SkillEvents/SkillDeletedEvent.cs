namespace MediaLink.Domain.Events.SkillEvents;

public class SkillDeletedEvent : BaseEvent
{
    public SkillDeletedEvent(Skill skill)
    {
        Skill = skill;
    }

    public Skill Skill { get; }
}