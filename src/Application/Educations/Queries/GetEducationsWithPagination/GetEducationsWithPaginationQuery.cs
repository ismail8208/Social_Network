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

namespace MediaLink.Application.Educations.Queries.GetEducationsWithPagination;
[Authorize(Roles = "member")]
public record GetEducationsWithPaginationQuery : IRequest<PaginatedList<EducationDto>>
{
    public int UserId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetEducationsWithPaginationQueryHandler : IRequestHandler<GetEducationsWithPaginationQuery, PaginatedList<EducationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEducationsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<EducationDto>> Handle(GetEducationsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Educations
              .Where(E => E.UserId == request.UserId)
              .Include(u => u.User)
              .OrderBy(E => E.Created)
              .ProjectTo<EducationDto>(_mapper.ConfigurationProvider)
              .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
