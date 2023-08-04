using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Events.ProjectEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Projects.Commands.DeleteProject;
[Authorize(Roles = "member")]
public record DeleteProjectCommand(int Id) : IRequest;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(entity));
        }

        entity.IsDeleted = true;

        entity.AddDomainEvent(new ProjectDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

