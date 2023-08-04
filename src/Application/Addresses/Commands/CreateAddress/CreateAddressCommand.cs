using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Events.AddressEvents;
using MediatR;

namespace MediaLink.Application.Addresses.Commands.CreateAddress;
[Authorize(Roles = "member")]
public record CreateAddressCommand : IRequest<int>
{
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public int UserId { get; set; }
}

public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateAddressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = new Address
        {
            Country = request.Country,
            City = request.City,
            Street = request.Street,
            UserId = request.UserId
        };

        entity.AddDomainEvent(new AddressCreatedEvent(entity));

        _context.Addresses.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}