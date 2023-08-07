using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.CVService.DTOs;
public class EducationCV : IMapFrom<Education>
{
    public string? Title { get; set; }
    public string? Level { get; set; }
}
