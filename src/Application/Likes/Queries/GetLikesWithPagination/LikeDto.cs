using AutoMapper;
using MediaLink.Application.Comments.Queries.GetCommentsWithPagination;
using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Likes.Queries.GetLikesWithPagination;
public class LikeDto : IMapFrom<Like>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string? UserName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Like, LikeDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName));
    }
}
