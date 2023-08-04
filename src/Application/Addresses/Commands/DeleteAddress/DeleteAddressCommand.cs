using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Events.AddressEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Addresses.Commands.DeleteAddress;

[Authorize(Roles = "member")]
public record DeleteAddressCommand (int Id) : IRequest;


public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAddressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {

        var entity = await _context.Addresses
            .Where(C => C.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(entity));
        }

        _context.Addresses.Remove(entity);

        entity.AddDomainEvent(new AddressDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
