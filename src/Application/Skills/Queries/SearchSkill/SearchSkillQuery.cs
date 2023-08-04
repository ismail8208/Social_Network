using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Common.Security;
using MediatR;

namespace MediaLink.Application.Skills.Queries.SearchSkill;
[Authorize(Roles = "member")]
public record SearchSkillQuery : IRequest<PaginatedList<SkillDto>>
{
    public string? Query { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class SearchSkillQueryHandler : IRequestHandler<SearchSkillQuery, PaginatedList<SkillDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchSkillQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<SkillDto>> Handle(SearchSkillQuery request, CancellationToken cancellationToken)
    {
        return await _context.Skills
           .Where(u => u.Title.StartsWith(request.Query) && u.IsDeleted == false)
           .ProjectTo<SkillDto>(_mapper.ConfigurationProvider)
           .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
