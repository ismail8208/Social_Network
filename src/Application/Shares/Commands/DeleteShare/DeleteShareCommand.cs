using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Events.ShareEvents; 
using MediatR;

namespace MediaLink.Application.Shares.Commands.DeleteShare;
[Authorize(Roles = "member")]
public record DeleteShareCommand(int Id) : IRequest;

public class DeleteShareCommandHandler : IRequestHandler<DeleteShareCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteShareCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteShareCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Shares
           .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Share), request.Id);
        }

        _context.Shares.Remove(entity);

        entity.AddDomainEvent(new ShareDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}