using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Posts.Queries.GetPost;
[Authorize(Roles = "member")]
public record GetPostQurey(int Id) : IRequest<PostDto>;
public class GetPostQureyHandler : IRequestHandler<GetPostQurey, PostDto>
{
    private readonly IApplicationDbContext _context;
    public GetPostQureyHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<PostDto> Handle(GetPostQurey request, CancellationToken cancellationToken)
    {
        var post =  await _context.Posts.Include(u => u.User).FirstOrDefaultAsync(p =>p.Id == request.Id && p.IsDeleted == false);
        if (post == null)
        {
            throw new NotFoundException(nameof(Post), request.Id);
        }

        var entity = new PostDto
        {
            Id= post.Id,
            Content= post.Content,
            ImageURL= post.ImageURL,
            NumberOfComments= post.NumberOfComments,
            NumberOfLikes = post.NumberOfLikes,
            UserId= post.UserId,
            VideoURL = post.VideoURL,
            UserName = post.User.UserName,
            Created = post.Created,
        };

        return entity;
    }
}