using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Events.EndorsementEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Endorsements.Commands.DeleteEndorsement;
[Authorize(Roles = "member")]
public record DeleteEndorsementCommand (int Id) : IRequest;

public class DeleteEndorsementCommandHandler : IRequestHandler<DeleteEndorsementCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteEndorsementCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteEndorsementCommand request, CancellationToken cancellationToken)
    {
/*        var entity = await _context.Endorsements
            .Where(E => E.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);*/
        var entity = await _context.Endorsements.FirstOrDefaultAsync(e => e.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(entity));
        }

        _context.Endorsements.Remove(entity);

        entity.AddDomainEvent(new EndrosementDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

