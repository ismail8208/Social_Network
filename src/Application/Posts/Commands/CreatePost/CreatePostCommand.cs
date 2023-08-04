using System.Data;
using MediaLink.Application.Common.FilesHandling;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Enums;
using MediaLink.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace MediaLink.Application.Posts.Commands.CreatePost;
[Authorize(Roles = "member")]
public record CreatePostCommand : IRequest<int>
{
    public string? Content { get; set; }
    public IFormFile? Image { get; set; }
    public IFormFile? Video { get; set; }
    public int UserId { get; set; }
}

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreatePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var entity = new Post
        {
            Content = request.Content,
            VideoURL = await SaveFile.Save(FileType.video, request.Video),
            UserId = request.UserId,
            ImageURL = await SaveFile.Save(FileType.image, request.Image)
        };
        

        entity.AddDomainEvent(new PostCreatedEvent(entity));
      
        _context.Posts.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
