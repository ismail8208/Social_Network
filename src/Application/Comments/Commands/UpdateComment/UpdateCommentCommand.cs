using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediatR;

namespace MediaLink.Application.Comments.Commands.UpdateComment;
[Authorize(Roles = "member")]
public record UpdateCommentCommand : IRequest
{
    public int Id { get; init; }
    public string? Content { get; init; }
}
public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCommentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Comments
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Comment),request.Id);
        }

        entity.Content = request.Content;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
