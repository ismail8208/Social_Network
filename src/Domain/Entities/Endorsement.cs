namespace MediaLink.Domain.Entities;

public class Endorsement : BaseAuditableEntity
{
    public int SkillId { get; set; }
    public Skill? Skill { get; set; }
    public int UserId { get; set; }
    public InnerUser? User { get; set; }
}
