using System.Data;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Events.ExperienceEvents;
using MediatR;

namespace MediaLink.Application.Experiences.Commands.CreateExperience;
[Authorize(Roles = "member")]
public record CreateExperienceCommand : IRequest <int>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int ExperienceDate { get; set; }
    public string? CompanyName { get; set; }
    public DateTime StartedTime { get; set; }
    public int UserId { get; set; }
}

public class CreateExperienceCommandHandler : IRequestHandler<CreateExperienceCommand, int>
{

    private readonly IApplicationDbContext _context;

    public CreateExperienceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateExperienceCommand request, CancellationToken cancellationToken)
    {
        var entity = new Experience
        {
            Title = request.Title,
            Description = request.Description,
            ExperienceDate = request.ExperienceDate,
            CompanyName = request.CompanyName,
            StartedTime = request.StartedTime,
            UserId = request.UserId
        };

        entity.AddDomainEvent(new ExperienceCreatedEvent(entity));

        _context.Experiences.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
