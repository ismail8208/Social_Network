namespace MediaLink.Domain.Entities;

public class Post : BaseAuditableEntity
{

    public string? Content { get; set; }
    public string? ImageURL { get; set; }
    public string? VideoURL { get; set; }
    public int NumberOfLikes { get; set; }
    public int NumberOfComments { get; set; }
    public int UserId { get; set; }
    public InnerUser? User { get; set; }

    // Navigation properties
    public List<Like> Likes { get; set; } = new List<Like>();
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public List<Share> Shares { get; set; } = new List<Share>();
    public bool IsDeleted { get; set; } = false;

}

