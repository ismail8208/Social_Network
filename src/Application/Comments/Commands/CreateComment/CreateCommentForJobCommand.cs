using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Events.CommentEvents;
using MediatR;

namespace MediaLink.Application.Comments.Commands.CreateComment;
[Authorize(Roles = "member")]
public record CreateCommentForJobCommand : IRequest<int>
{
    public string? Content { get; set; }
    public int JobId { get; set; }
    public int UserId { get; set; }
}
public class CreateCommentForJobCommandHandler : IRequestHandler<CreateCommentForJobCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCommentForJobCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateCommentForJobCommand request, CancellationToken cancellationToken)
    {
        var entity = new Comment
        {
            Content = request.Content,
            UserId = request.UserId,
            JobId = request.JobId
        };

        entity.AddDomainEvent(new CommentCreatedEvent(entity));

        _context.Comments.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
