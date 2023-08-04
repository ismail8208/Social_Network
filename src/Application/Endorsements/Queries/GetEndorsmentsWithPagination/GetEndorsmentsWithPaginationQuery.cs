using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Endorsements.Queries.GetEndorsmentsWithPagination;
[Authorize(Roles = "member")]
public record GetEndorsmentsWithPaginationQuery : IRequest<PaginatedList<EndorsmentDto>>
{
    public int SkillId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetEndorsmentsWithPaginationQueryHandler : IRequestHandler<GetEndorsmentsWithPaginationQuery, PaginatedList<EndorsmentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEndorsmentsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<EndorsmentDto>> Handle(GetEndorsmentsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Endorsements
            .Where(E => E.SkillId == request.SkillId)
            .Include(u => u.User)
            .OrderBy(E => E.Created)
            .ProjectTo<EndorsmentDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
