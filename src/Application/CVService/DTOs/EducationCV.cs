using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.CVService.DTOs;
public class EducationCV : IMapFrom<Education>
{
    public string? Level { get; set; }
    public string? Title { get; set; }
}
