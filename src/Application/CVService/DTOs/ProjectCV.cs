using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.CVService.DTOs; 
public class ProjectCV : IMapFrom<Project>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Link { get; set; }
}
