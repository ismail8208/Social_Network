using System.Data;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediatR;

namespace MediaLink.Application.Follows.Commands.Follow;
[Authorize(Roles = "member")]
public record FollowCommand : IRequest
{
    public int UserId { get; set; }
    public int SpecificUserId { get; set; }
}

public class FollowCommandHandler : IRequestHandler<FollowCommand>
{
    private readonly IApplicationDbContext _context;

    public FollowCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(FollowCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Follow
        {
            FollowingID = request.UserId,
            FollowerID = request.SpecificUserId,
        };

        _context.Follows.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}