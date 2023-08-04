namespace MediaLink.Domain.Entities;
public class Education : BaseAuditableEntity
{
    public string? Level { get; set; }
    public string? Title { get; set; }
    public int UserId { get; set; }
    public InnerUser? User { get; set; }
}
