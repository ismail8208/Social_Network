using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.CVService.DTOs;
public class ExperienceCV : IMapFrom<Experience>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? CompanyName { get; set; }
    public DateTime StartedTime { get; set; }
    public int? ExperienceDate { get; set; }
}
