namespace MediaLink.Domain.Entities;

public class Like : BaseAuditableEntity
{
    public int? UserId { get; set; }
    public InnerUser? User { get; set; }

    public int? PostId { get; set; }
    public Post? Post { get; set; }

    public int? JobId { get; set; }
    public Job? Job { get; set; }
}

