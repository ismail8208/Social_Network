using System.Data;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Events.ShareEvents;
using MediatR;

namespace MediaLink.Application.Shares.Commands.CreateShare;
[Authorize(Roles = "member")]
public record CreateShareCommand : IRequest<int>
{
    public int UserId { get; set; }
    public int PostId { get; set; }
}

public class CreateShareCommandHandler : IRequestHandler<CreateShareCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateShareCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateShareCommand request, CancellationToken cancellationToken)
    {
        var entity = new Share
        {
            UserId = request.UserId,
            PostId = request.PostId
        };

        entity.AddDomainEvent(new ShareCreatedEvent(entity));

        _context.Shares.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
