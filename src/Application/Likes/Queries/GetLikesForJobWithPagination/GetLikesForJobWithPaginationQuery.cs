using AutoMapper;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Models;
using MediatR;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Data;
using MediaLink.Application.Common.Security;

namespace MediaLink.Application.Likes.Queries.GetLikesForJobWithPagination;
[Authorize(Roles = "member")]
public record GetLikesForJobWithPaginationQuery : IRequest<PaginatedList<LikeForJobDto>>
{
    public int JobId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetLikesForJobWithPaginationQueryHandler : IRequestHandler<GetLikesForJobWithPaginationQuery, PaginatedList<LikeForJobDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLikesForJobWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<LikeForJobDto>> Handle(GetLikesForJobWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Likes
            .Where(j => j.JobId == request.JobId)
            .OrderBy(j => j.Created)
            .Include(u => u.User)
            .ProjectTo<LikeForJobDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
