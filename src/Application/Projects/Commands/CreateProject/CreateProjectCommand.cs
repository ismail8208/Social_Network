using System.Data;
using MediaLink.Application.Common.FilesHandling;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Enums;
using MediaLink.Domain.Events.ProjectEvents;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MediaLink.Application.Projects.Commands.CreateProject;
[Authorize(Roles = "member")]
public record CreateProjectCommand : IRequest<int>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
    public string? Link { get; set; }
    public int UserId { get; set; }
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = new Project
        {
            Title = request.Title,
            Description = request.Description,
            ImageURL = await SaveFile.Save(FileType.image, request.Image),
            Link = request.Link,
            UserId = request.UserId
        };

        entity.AddDomainEvent(new ProjectCreatedEvent(entity));

        _context.Projects.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}