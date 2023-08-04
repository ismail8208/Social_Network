using AutoMapper;
using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Projects.Queries.GetProjectsWithPagination;

public class ProjectDto : IMapFrom<Project>
{
    public int Id { get; set; }
    public int? ExperienceId { get; set; }
    public string? Content { get; set; }
    public string? ImageURL { get; set; }
    public string? Link { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Project, ProjectDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName));
    }
}
