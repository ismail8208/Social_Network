using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Addresses.Queries.SearchAddress;
[Authorize(Roles = "member")]
public record SearchAddressQuery : IRequest<PaginatedList<AddressDto>>
{
    public string? Query { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
public class SearchAddressQueryHandler : IRequestHandler<SearchAddressQuery, PaginatedList<AddressDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchAddressQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<AddressDto>> Handle(SearchAddressQuery request, CancellationToken cancellationToken)
    {
        return await _context.Addresses
            .Where(u => u.Country.StartsWith(request.Query) || u.City.StartsWith(request.Query) || u.Street.StartsWith(request.Query))
            .Include(u => u.User)
            .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
