using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Follows.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Likes.Queries.GetLiksOfPost;
public record GetLikeOfPostsQuery(int PostId) : IRequest<List<BriefUserDto>>;
public class GetLikeOfPostsQueryHandler : IRequestHandler<GetLikeOfPostsQuery, List<BriefUserDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLikeOfPostsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<BriefUserDto>> Handle(GetLikeOfPostsQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Likes.Include(u => u.User)
            .Where(c => c.PostId == request.PostId).Select(u => u.User)
            .ProjectTo<BriefUserDto>(_mapper.ConfigurationProvider).ToListAsync();

        return users;
    }
}