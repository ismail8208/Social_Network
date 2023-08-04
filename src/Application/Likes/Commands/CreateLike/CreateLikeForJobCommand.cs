using System.Data;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediatR;

namespace MediaLink.Application.Likes.Commands.CreateLike;
[Authorize(Roles = "member")]
public record CreateLikeForJobCommand : IRequest<int>
{
    public int UserId { get; set; }
    public int JobId { get; set; }
}

public class CreateLikeForJobCommandHandler : IRequestHandler<CreateLikeForJobCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateLikeForJobCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateLikeForJobCommand request, CancellationToken cancellationToken)
    {
        var entity = new Like
        {
            UserId = request.UserId,
            JobId = request.JobId
        };

        _context.Likes.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}