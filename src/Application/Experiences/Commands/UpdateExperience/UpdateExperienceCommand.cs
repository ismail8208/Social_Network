using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaLink.Application.Addresses.Commands.UpdateAddress;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediatR;

namespace MediaLink.Application.Experiences.Commands.UpdateExperience;
[Authorize(Roles = "member")]
public record UpdateExperienceCommand : IRequest
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int ExperienceDate { get; set; }
    public string? CompanyName { get; set; }
    public DateTime StartedTime { get; set; }
    public int UserId { get; set; }
}

public class UpdateExperienceCommandHandler : IRequestHandler<UpdateExperienceCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateExperienceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateExperienceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Experiences
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Experience), request.Id);
        }

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.ExperienceDate = request.ExperienceDate;
        entity.UserId = request.UserId;
        entity.CompanyName = request.CompanyName;
        entity.StartedTime = request.StartedTime;


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}