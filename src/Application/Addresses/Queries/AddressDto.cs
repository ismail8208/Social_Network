using AutoMapper;
using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Addresses.Queries;
public class AddressDto : IMapFrom<Address>
{
    public string? FullAddress { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Address, AddressDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName))
            .ForMember(d => d.FullAddress, opt => opt.MapFrom(s => (s.Country+", "+s.City+", "+s.Street+".")));
    }
}
