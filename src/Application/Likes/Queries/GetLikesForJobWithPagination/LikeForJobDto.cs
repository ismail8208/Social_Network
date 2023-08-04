using AutoMapper;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Likes.Queries.GetLikesWithPagination;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Likes.Queries.GetLikesForJobWithPagination;
public class LikeForJobDto : IMapFrom<Like>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int JobId { get; set; }
    public string? UserName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Like, LikeDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName));
    }
}