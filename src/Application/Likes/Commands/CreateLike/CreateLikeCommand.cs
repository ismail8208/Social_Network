using System.Data;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Events.LikeEvents;
using MediatR;

namespace MediaLink.Application.Likes.Commands.CreateLike;
[Authorize(Roles = "member")]
public record CreateLikeCommand : IRequest<int>
{
    public int UserId { get; set; }
    public int PostId { get; set; }
}

public class CreateLikeCommandHandler : IRequestHandler<CreateLikeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateLikeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateLikeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Like
        {
            UserId = request.UserId,
            PostId = request.PostId
        };

        entity.AddDomainEvent(new LikeCreatedEvent(entity));

        _context.Likes.Add(entity);
        
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

}
