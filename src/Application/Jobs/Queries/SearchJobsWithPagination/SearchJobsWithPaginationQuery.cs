using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Common.Security;
using MediatR;

namespace MediaLink.Application.Jobs.Queries.GetJobsWithPagination;
[Authorize(Roles = "member")]
public record SearchJobsWithPaginationQuery : IRequest<PaginatedList<JobDto>>
{
    public string? Query { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 5;
}

public class SearchJobsWithPaginationQueryHandler : IRequestHandler<SearchJobsWithPaginationQuery, PaginatedList<JobDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchJobsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<JobDto>> Handle(SearchJobsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Jobs
        .Where(u => u.Title.Contains(request.Query) && u.IsDeleted == false)
        .ProjectTo<JobDto>(_mapper.ConfigurationProvider)
        .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}