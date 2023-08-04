using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Projects.Queries.GetProjectsWithPagination;
[Authorize(Roles = "member")]
public record GetProjectsWithPaginationQuery : IRequest<PaginatedList<ProjectDto>>
{
    public int UserId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProjectsWithPaginationQueryHandler : IRequestHandler<GetProjectsWithPaginationQuery, PaginatedList<ProjectDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProjectsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<ProjectDto>> Handle(GetProjectsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Projects
           .Where(P => P.UserId == request.UserId && P.IsDeleted == false)
           .OrderBy(P => P.Created)
           .Include(u => u.User)
           .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
           .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
