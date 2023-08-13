using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Application.Dashboard.UsersInfo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Dashboard.PostsInfo;
[Authorize(Roles = "Administrator")]

public record GetPostInfo : IRequest<PostInfoDto>
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}
public class GetPostInfoHnadler : IRequestHandler<GetPostInfo, PostInfoDto>
{
    private readonly IApplicationDbContext _context;

    public GetPostInfoHnadler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<PostInfoDto> Handle(GetPostInfo request, CancellationToken cancellationToken)
    {

        var dateRange = Enumerable.Range(0, 1 + request.DateTo.Subtract(request.DateFrom).Days).Select(offset => request.DateFrom.AddDays(offset)).ToList();

        var postInfo = new PostInfoDto
        {
            DateTimes = dateRange,
            NumberOfPosts = dateRange
                .Select(date =>
                    _context.Posts.Count(u =>
                        u.Created.Date == date.Date
                    )
            )
                .ToList()
        };
        postInfo.NumberOfAllPosts = await _context.Posts.CountAsync();
        return postInfo;
    }
}

