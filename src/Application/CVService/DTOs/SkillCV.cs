using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.CVService.DTOs; 
public class SkillCV : IMapFrom<Skill>
{
    public string? Title { get; set; }
}
