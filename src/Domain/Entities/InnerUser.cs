namespace MediaLink.Domain.Entities;

public class InnerUser : BaseAuditableEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Summary { get; set; }
    public Address? Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? ProfileImage { get; set; }
    public string? specialization { get; set; }
    // Navigation properties
    public List<Skill>? Skills { get; set; }
    public List<Project>? Projects { get; set; }
    public List<Experience>? Experiences { get; set; }
    public List<Endorsement>? Endorsements { get; set; }
    public List<Education>? Educations { get; set; }
    public List<Post>? Posts { get; set; }
    public List<Follow>? Followers { get; set; }
    public List<Follow>? Followings { get; set; }
    public List<Share>? SharedPosts { get; set; }
    public List<Like>? Likes { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<CV>? CVs { get; set; }
    public List<Notification>? Notifications { get; set; }
    public bool IsDeleted { get; set; } = false;

}

