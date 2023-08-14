using AutoMapper;
using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Addresses.Queries;
public class AddressDto : IMapFrom<Address>
{
    public string? FullAddress { get; set; }
    public int UserId { get; set; }
}
