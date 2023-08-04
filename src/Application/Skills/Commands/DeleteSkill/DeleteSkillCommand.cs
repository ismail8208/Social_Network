using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Events.SkillEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Skills.Commands.DeleteSkill;
[Authorize(Roles = "member")]
public record DeleteSkillCommand(int Id) : IRequest;

public class DeleteSkillCommandHandler : IRequestHandler<DeleteSkillCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteSkillCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Skills
           .Where(S => S.Id == request.Id)
           .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(entity));
        }

        entity.IsDeleted = true;

        entity.AddDomainEvent(new SkillDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}