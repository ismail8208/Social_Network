using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Likes.Queries.GetLikesWithPagination;
[Authorize(Roles = "member")]
public record GetLikesWithPaginationQuery :IRequest<PaginatedList<LikeDto>>
{
    public int PostId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetLikesWithPaginationQueryHandler : IRequestHandler<GetLikesWithPaginationQuery, PaginatedList<LikeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLikesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<LikeDto>> Handle(GetLikesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Likes
            .Where(L => L.PostId == request.PostId)
            .OrderBy(L => L.Created)
            .Include(u => u.User)
            .ProjectTo<LikeDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}