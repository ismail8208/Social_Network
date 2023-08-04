using System.Data;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Events.EndorsementEvents;
using MediatR;

namespace MediaLink.Application.Endorsements.Commands.CreateEndorsement;
[Authorize(Roles = "member")]
public record CreateEndorsementCommand : IRequest<int>
{
    public int SkillId { get; set; }
    public int UserId { get; set; }
}

public class CreateEndorsementCommandHandler : IRequestHandler<CreateEndorsementCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateEndorsementCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateEndorsementCommand request, CancellationToken cancellationToken)
    {
        var entity = new Endorsement
        {
            
            SkillId = request.SkillId,
            UserId = request.UserId
        };

        entity.AddDomainEvent(new EndorsementCreatedEvent(entity));

        _context.Endorsements.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
