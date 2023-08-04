using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.CVService.DTOs;
public class ExperienceCV : IMapFrom<Experience>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? ExperienceDate { get; set; }
}
