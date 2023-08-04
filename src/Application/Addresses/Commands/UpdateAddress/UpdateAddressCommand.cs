using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MediaLink.Application.Addresses.Commands.UpdateAddress;
[Authorize(Roles = "member")]
public record UpdateAddressCommand : IRequest
{
    public int Id { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
}

public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateAddressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Addresses
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Address), request.Id);
        }

        entity.Country = request.Country;
        entity.City = request.City;
        entity.Street = request.Street;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
