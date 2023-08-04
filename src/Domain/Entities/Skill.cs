namespace MediaLink.Domain.Entities;

public class Skill : BaseAuditableEntity
{
    public string? Title { get; set; }
    public List<Endorsement>? Endorsements { get; set; }
    public int? UserId { get; set; }
    public InnerUser? User { get; set; }

    public int? JobId { get; set; }
    public Job? Job { get; set; }
    public bool IsDeleted { get; set; } = false;
}

