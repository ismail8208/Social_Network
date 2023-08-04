using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Addresses.Queries.GetAddressByUserId;
[Authorize(Roles = "member")]
public record GetAddressQuery(int id) : IRequest<AddressDto>;


public class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, AddressDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAddressQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<AddressDto> Handle(GetAddressQuery request, CancellationToken cancellationToken)
    {
        var address =  await _context.Addresses
            .Include(u => u.User)
            .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(a=>a.UserId == request.id);
        if (address == null)
        {
            throw new NotFoundException();
        }
        return address;
    }
}
