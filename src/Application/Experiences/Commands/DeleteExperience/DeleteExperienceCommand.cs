using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Events.ExperienceEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Experiences.Commands.DeleteExperience;
[Authorize(Roles = "member")]
public record DeleteExperienceCommand(int Id) : IRequest;

public class DeleteExperienceCommandHandler : IRequestHandler<DeleteExperienceCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteExperienceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteExperienceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Experiences
            .Where(E => E.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(entity));
        }

        _context.Experiences.Remove(entity);

        entity.AddDomainEvent(new ExperienceDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
