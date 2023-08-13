using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Posts.Queries.LatestNews;
[Authorize(Roles = "Administrator")]
[Authorize(Roles = "member")]
public record LatestNewsQuery : IRequest<PaginatedList<PostDto>>
{
    public int UserId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 15;
}
public class LatestNewsQueryHandler : IRequestHandler<LatestNewsQuery, PaginatedList<PostDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public LatestNewsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PostDto>> Handle(LatestNewsQuery request, CancellationToken cancellationToken)
    {
        var followedUserIds = await _context.Follows
            .Where(f => f.FollowingID == request.UserId)
            .Select(f => f.FollowerID)
            .ToListAsync();

        return await _context.Posts
            .Where(p => followedUserIds.Contains(p.UserId) && p.IsDeleted == false || p.UserId == request.UserId)
            .Include(u => u.User)
            .Include(p => p.Likes)
            .OrderByDescending(p => p.Created)
            .ThenByDescending(p => p.Likes.Count)
            .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
