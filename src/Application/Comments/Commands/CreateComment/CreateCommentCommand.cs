using System.Data;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Events.CommentEvents;
using MediatR;

namespace MediaLink.Application.Comments.Commands.CreateComment;
[Authorize(Roles = "member")]
public record CreateCommentCommand : IRequest<int>
{
    public string? Content { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
}

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCommentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Comment
        {
            Content = request.Content,
            PostId = request.PostId,
            UserId = request.UserId
        };

        entity.AddDomainEvent(new CommentCreatedEvent(entity));
       
        _context.Comments.Add(entity);
        
        await _context.SaveChangesAsync(cancellationToken);
       
        return entity.Id;
    }
}
