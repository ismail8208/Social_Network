using System.Data;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Jobs.Queries.GetJob;
[Authorize(Roles = "member")]
public record GetJobQuery(int Id) : IRequest<JobDto>;
public class GetJobQueryHandler : IRequestHandler<GetJobQuery, JobDto>
{
    private readonly IApplicationDbContext _context;

    public GetJobQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<JobDto> Handle(GetJobQuery request, CancellationToken cancellationToken)
    {
        var job = await _context.Jobs.Include(u => u.User).FirstOrDefaultAsync(p => p.Id == request.Id && p.IsDeleted == false);
        if (job == null)
        {
            throw new NotFoundException(nameof(Job), request.Id);
        }

        var entity = new JobDto
        {
            Id= job.Id,
            Description= job.Description,
            Title= job.Title,
            UserName = job.User.UserName
        };

        return entity;
    }
}