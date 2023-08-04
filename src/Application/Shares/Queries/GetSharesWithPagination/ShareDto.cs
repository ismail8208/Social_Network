using AutoMapper;
using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Shares.Queries.GetSharesWithPagination;
public class ShareDto : IMapFrom<Share>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string? UserName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Share, ShareDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName));
    }
}
