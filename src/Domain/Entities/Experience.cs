namespace MediaLink.Domain.Entities;

public class Experience : BaseAuditableEntity
{
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int ExperienceDate { get; set; }
    public string? CompanyName { get; set; }
    public DateTime StartedTime { get; set; }
    public int UserId { get; set; }
    public InnerUser? User { get; set; }
}

