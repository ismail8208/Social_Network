namespace MediaLink.Domain.Entities;

public class Follow : BaseEntity
{
    public int FollowerID { get; set; }
    public int FollowingID { get; set; }

    // Navigation properties
    public InnerUser? Follower { get; set; }
    public InnerUser? Followee { get; set; }
}

