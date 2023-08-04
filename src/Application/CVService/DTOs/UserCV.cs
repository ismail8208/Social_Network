using AutoMapper;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Jobs.Queries;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.CVService.DTOs; 
public class UserCV : IMapFrom<InnerUser>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? ProfileImage { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<InnerUser, UserCV>()
            .ForMember(d => d.Address, opt => opt.MapFrom(s => (s.Address.Country+ ", " + s.Address.City + ", "+ s.Address.Street + ". ")));
    }
}
