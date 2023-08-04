using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediatR;

namespace MediaLink.Application.Educations.Commands.UpdateEducation;
[Authorize(Roles = "member")]
public record UpdateEducationCommand : IRequest
{
    public int Id { get; set; }
    public string? Level { get; set; }
    public string? Title { get; set; }
    public int UserId { get; set; }
}

public class UpdateEducationCommandHandler : IRequestHandler<UpdateEducationCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateEducationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateEducationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Educations
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Education), request.Id);
        }

        entity.Level = request.Level;
        entity.Title = request.Title;
        entity.UserId = request.UserId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}