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

namespace MediaLink.Application.Skills.Queries.GetSkillsWithPagination;
[Authorize(Roles = "member")]
public record GetSkillsWithPaginationQuery : IRequest<PaginatedList<Skill>>
{
    public int UserId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetSkillsWithPaginationQueryHandler : IRequestHandler<GetSkillsWithPaginationQuery, PaginatedList<Skill>>
{
    private readonly IApplicationDbContext _context;
    public GetSkillsWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<PaginatedList<Skill>> Handle(GetSkillsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Skills
              .Where(S => S.UserId == request.UserId && S.IsDeleted == false)
              .OrderBy(S => S.Created)
              .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
