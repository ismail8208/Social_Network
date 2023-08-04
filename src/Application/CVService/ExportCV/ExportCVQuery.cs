using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.CVService.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.CVService.ExportCV;
public record ExportCVQuery(int UserId) : IRequest<CV>;

public class ExportCVQueryHandler : IRequestHandler<ExportCVQuery, CV>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ExportCVQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<CV> Handle(ExportCVQuery request, CancellationToken cancellationToken)
    {
        var personalData = await _context.InnerUsers
            .Include(a => a.Address)
            .Where(u => u.Id == request.UserId)
            .ProjectTo<UserCV>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();

        if (personalData == null)
        {
            throw new NotFoundException();
        }

        var skills = await _context.Skills
            .Where(u => u.UserId == request.UserId)
            .ProjectTo<SkillCV>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var experiences = await _context.Experiences
            .Where(u => u.UserId == request.UserId)
            .ProjectTo<ExperienceCV>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var projects = await _context.Projects
            .Where(u => u.UserId == request.UserId)
            .ProjectTo<ProjectCV>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var educations = await _context.Educations
            .Where(u => u.UserId == request.UserId)
            .ProjectTo<EducationCV>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new CV(personalData, skills, experiences, projects, educations);
    }
}

