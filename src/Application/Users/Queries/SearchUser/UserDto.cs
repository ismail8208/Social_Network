using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Users.Queries.FindUser;
public class UserDto : IMapFrom<InnerUser>
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? ProfileImage { get; set; }
    public string? Role { get; set; }
    public string? Summary { get; set; }
    public int NumberOfFollowers { get; set; }
    public int NumberOfFollowings { get; set; }
    public int NumberOfPosts { get; set; }
    public string? specialization { get; set; }

}
