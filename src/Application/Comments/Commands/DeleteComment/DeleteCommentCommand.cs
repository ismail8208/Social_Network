using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Events.CommentEvents;
using MediatR;

namespace MediaLink.Application.Comments.Commands.DeleteComment;
[Authorize(Roles = "member")]
public record DeleteCommentCommand(int Id) : IRequest;
public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCommentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Comments.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(entity));
        }
       
        _context.Comments.Remove(entity);

        entity.AddDomainEvent(new CommentDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
