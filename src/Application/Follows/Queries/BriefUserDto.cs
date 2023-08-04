using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Follows.Queries;
public class BriefUserDto : IMapFrom<InnerUser>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? ProfileImage { get; set; }
}
