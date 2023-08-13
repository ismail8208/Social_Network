using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Common.Security;
using MediaLink.Application.Posts.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Jobs.Queries.GetJobsWithPagination;
[Authorize(Roles = "Administrator")]
[Authorize(Roles = "member")]

public record GetJobsWithPaginationQuery : IRequest<PaginatedList<JobDto>>
{
    public int UserId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
public class GetJobsWithPaginationQueryHandler : IRequestHandler<GetJobsWithPaginationQuery, PaginatedList<JobDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetJobsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<JobDto>> Handle(GetJobsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.Jobs
            .Where(x => x.UserId == request.UserId && x.IsDeleted == false)
            .Include(u => u.User)
            .OrderByDescending(x => x.Created)
            .ProjectTo<JobDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return posts;
    }
}

