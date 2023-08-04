using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Shares.Queries.GetSharesWithPagination;
[Authorize(Roles = "member")]
public record GetSharesWithPaginationQuery : IRequest<PaginatedList<ShareDto>>
{
    public int PostId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetSharesWithPaginationQueryHandler : IRequestHandler<GetSharesWithPaginationQuery, PaginatedList<ShareDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSharesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<ShareDto>> Handle(GetSharesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Shares
            .Where(S => S.PostId == request.PostId)
            .OrderBy(S => S.Created)
            .Include(u => u.User)
            .ProjectTo<ShareDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}