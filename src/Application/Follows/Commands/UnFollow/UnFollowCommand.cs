using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Follows.Commands.UnFollow;
[Authorize(Roles = "member")]
public record UnFollowCommand : IRequest
{
    public int UserId { get; set; }
    public int SpecificUserId { get; set; }
}

public class UnFollowCommandHandler : IRequestHandler<UnFollowCommand>
{
    private readonly IApplicationDbContext _context;

    public UnFollowCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(UnFollowCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Follows.FirstOrDefaultAsync(u => u.FollowerID == request.SpecificUserId && u.FollowingID == request.UserId);
        
        if (entity == null)
        {
            throw new NotFoundException();
        }

        _context.Follows.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}