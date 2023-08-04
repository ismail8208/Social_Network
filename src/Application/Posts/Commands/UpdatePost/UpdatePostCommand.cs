using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.FilesHandling;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MediaLink.Application.Posts.Commands.UpdatePost;
[Authorize(Roles = "member")]
public record UpdatePostCommand : IRequest
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public IFormFile? Image { get; set; }
    public IFormFile? Video { get; set; }
}


public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdatePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Posts.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Post), request.Id);
        }

        entity.Content= request.Content;
        entity.ImageURL= await SaveFile.Save(FileType.image, request.Image);
        entity.VideoURL= await SaveFile.Save(FileType.video, request.Video);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
