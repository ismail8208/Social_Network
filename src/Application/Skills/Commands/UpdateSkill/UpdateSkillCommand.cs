using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediatR;

namespace MediaLink.Application.Skills.Commands.UpdateSkill;
[Authorize(Roles = "member")]
public record UpdateSkillCommand : IRequest
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int UserId { get; set; }
}

public class UpdateSkillCommandHandler : IRequestHandler<UpdateSkillCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSkillCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Skills
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Skill), request.Id);
        }

        entity.Title = request.Title;
        entity.UserId = request.UserId;
       
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}