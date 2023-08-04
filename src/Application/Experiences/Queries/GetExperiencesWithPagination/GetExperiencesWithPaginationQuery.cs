using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Experiences.Queries.GetExperiencesWithPagination;
[Authorize(Roles = "member")]
public record GetExperiencesWithPaginationQuery : IRequest<PaginatedList<ExperienceDto>>
{
    public int UserId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetExperiencesWithPaginationQueryHandler : IRequestHandler<GetExperiencesWithPaginationQuery, PaginatedList<ExperienceDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetExperiencesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<ExperienceDto>> Handle(GetExperiencesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Experiences
            .Where(E => E.UserId == request.UserId)
            .OrderBy(E => E.Created)
            .Include(u => u.User)
            .Include(p => p.Project)
            .ProjectTo<ExperienceDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
