using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Experiences.Commands.AddProjectToExperience;
[Authorize(Roles = "member")]
public record AddProjectCommand : IRequest
{
    public int ProjectId { get; set; }
    public int ExperienceId { get; set; }
}

public class AddProjectHndler : IRequestHandler<AddProjectCommand>
{
    private readonly IApplicationDbContext _context;

    public AddProjectHndler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(AddProjectCommand request, CancellationToken cancellationToken)
    {
        var experience = await _context.Experiences.FirstOrDefaultAsync(e => e.Id == request.ExperienceId);
        if (experience == null)
        {
            throw new NotFoundException();
        }

        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId);
        if (project == null)
        {
            throw new NotFoundException();
        }

        experience.Project = project;
        experience.ProjectId = project.Id;
        _context.Experiences.Update(experience);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}