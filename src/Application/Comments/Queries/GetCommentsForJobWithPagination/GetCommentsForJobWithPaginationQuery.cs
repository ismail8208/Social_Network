using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Comments.Queries.GetCommentsWithPagination;
[Authorize(Roles = "member")]
public record GetCommentsForJobWithPaginationQuery :IRequest<PaginatedList<CommentForJobDto>>
{
    public int JobId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetCommentsForJobWithPaginationQueryHandler : IRequestHandler<GetCommentsForJobWithPaginationQuery, PaginatedList<CommentForJobDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCommentsForJobWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<CommentForJobDto>> Handle(GetCommentsForJobWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Comments
            .Where(C => C.JobId == request.JobId)
            .OrderBy(C => C.Created)
            .Include(u => u.User)
            .ProjectTo<CommentForJobDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}