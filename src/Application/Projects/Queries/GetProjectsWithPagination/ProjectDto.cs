using AutoMapper;
using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Projects.Queries.GetProjectsWithPagination;

public class ProjectDto : IMapFrom<Project>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public string? Link { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Project, ProjectDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName));
    }
}
