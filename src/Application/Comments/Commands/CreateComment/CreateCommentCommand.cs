using System.Data;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Application.Notification;
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
    private readonly IClientNotificationService _clientNotificationService;

    public CreateCommentCommandHandler(IApplicationDbContext context, IClientNotificationService clientNotificationService)
    {
        _context = context;
        _clientNotificationService = clientNotificationService;
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
        var not = new ClientNotificationDto
        {
            anything = "hello",
            Content = "test",
            UserId = request.UserId,
        };
        await _clientNotificationService.SendToAll(not);
        return entity.Id;
    }
}
