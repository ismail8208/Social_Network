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
public record GetCommentsWithPaginationQuery :IRequest<PaginatedList<CommentDto>>
{
    public int PostId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetCommentsWithPaginationQueryHandler : IRequestHandler<GetCommentsWithPaginationQuery, PaginatedList<CommentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCommentsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<CommentDto>> Handle(GetCommentsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Comments
            .Where(c =>c.PostId== request.PostId)
            .OrderBy(c => c.Created)
            .Include(u => u.User)
            .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber,request.PageSize);
    }
}