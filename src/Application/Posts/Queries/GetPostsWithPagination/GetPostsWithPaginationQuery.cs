using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Posts.Queries.GetPostsWithPagination;
[Authorize(Roles = "member")]
public record GetPostsWithPaginationQuery : IRequest<PaginatedList<PostDto>>
{
    public int UserId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetPostsWithPaginationQueryHandler : IRequestHandler<GetPostsWithPaginationQuery, PaginatedList<PostDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPostsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<PostDto>> Handle(GetPostsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.Posts
            .Where(x => x.UserId == request.UserId && x.IsDeleted == false)
            .Include(u => u.User)
            .OrderByDescending(x => x.Created)
            .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return posts;
    }
}
