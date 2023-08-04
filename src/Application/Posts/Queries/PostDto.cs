using AutoMapper;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Shares.Queries.GetSharesWithPagination;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Posts.Queries;
public class PostDto : IMapFrom<Post>
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public string? ImageURL { get; set; }
    public string? VideoURL { get; set; }
    public int NumberOfLikes { get; set; }
    public int NumberOfComments { get; set; }
    public DateTime Created { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Post, PostDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName));
    }
}
