using AutoMapper;
using MediaLink.Application.Addresses.Queries;
using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Educations.Queries;
public class EducationDto : IMapFrom<Education>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Level { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Education, EducationDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName));
    }
}
