using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Follows.Commands.CancelFollower;
[Authorize(Roles = "member")]
public record CancelFollowerCommand : IRequest
{
    public int UserId { get; set; }
    public int SpecificUserId { get; set; }
}

public class CancelFollowerCommandHandler : IRequestHandler<CancelFollowerCommand>
{
    private readonly IApplicationDbContext _context;

    public CancelFollowerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(CancelFollowerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Follows.FirstOrDefaultAsync(u => u.FollowerID == request.UserId && u.FollowingID == request.SpecificUserId);

        if (entity == null)
        {
            throw new NotFoundException();
        }

        _context.Follows.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
