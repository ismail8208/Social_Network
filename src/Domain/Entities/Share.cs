namespace MediaLink.Domain.Entities;
public class Share : BaseAuditableEntity 
{
    public string? Title { get; set; }
    public int UserId { get; set; }
    public InnerUser? User { get; set; }
    public int PostId { get; set; }
    public Post? Post { get; set; }
}
