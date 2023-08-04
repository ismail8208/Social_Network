using AutoMapper;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Posts.Queries;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Experiences.Queries.GetExperiencesWithPagination;
public class ExperienceDto : IMapFrom<Experience>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? ExperienceDate { get; set; }
    public int UserId { get; set; }
    public int? ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public string? UserName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Experience, ExperienceDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName))
            .ForMember(d => d.ProjectName, opt => opt.MapFrom(s => s.Project.Title));
    }


}
