using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediatR;

namespace MediaLink.Application.Follows.Queries.GetFollowingsWithPagination;
[Authorize(Roles = "member")]
public record GetFollowingsWithPaginationQuery : IRequest<PaginatedList<BriefUserDto>>
{
    public int UserId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetFollowingsWithPaginationQueryHandler : IRequestHandler<GetFollowingsWithPaginationQuery, PaginatedList<BriefUserDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFollowingsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<BriefUserDto>> Handle(GetFollowingsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Follows
           .Where(f => f.FollowingID == request.UserId)
           .Select(f => f.Follower)
           .ProjectTo<BriefUserDto>(_mapper.ConfigurationProvider)
           .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}