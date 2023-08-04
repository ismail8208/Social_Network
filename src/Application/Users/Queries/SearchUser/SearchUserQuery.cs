using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Common.Security;
using MediatR;

namespace MediaLink.Application.Users.Queries.FindUser;
/*[Authorize(Roles = "member")]
*/public record SearchUserQuery : IRequest<PaginatedList<UserDto>>
{
    public string? Query { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, PaginatedList<UserDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchUserQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<UserDto>> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        return await _context.InnerUsers
            .Where(u => (u.FirstName + " " + u.LastName).StartsWith(request.Query) && u.IsDeleted == false)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}