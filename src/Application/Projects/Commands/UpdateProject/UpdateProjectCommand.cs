using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.FilesHandling;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MediaLink.Application.Projects.Commands.UpdateProject;
[Authorize(Roles = "member")]
public record UpdateProjectCommand : IRequest
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
    public string? Link { get; set; }
    public int UserId { get; set; }
}

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Projects
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Project), request.Id);
        }

        entity.Description = request.Description;
        entity.ImageURL = await SaveFile.Save(FileType.image, request.Image);
        entity.Link = request.Link;
        entity.UserId = request.UserId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

}
