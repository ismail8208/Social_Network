using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Security;
using MediaLink.Application.CVService.DTOs;
using MediaLink.Application.CVService.ExportCV;
using MediaLink.Application.Users.Queries.FindUser;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.CVService.GetCVs;
[Authorize(Roles = "company")]
[Authorize(Roles = "Administrator")]
public record GetCVsQuery(int jobId) : IRequest<List<UserDto>>;

public class GetCVsQueryHandler : IRequestHandler<GetCVsQuery, List<UserDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCVsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<UserDto>> Handle(GetCVsQuery request, CancellationToken cancellationToken)
    {
       return await _context.CVs
            .Where(C => C.Position == request.jobId)
            .Include(u => u.User)
            .Select(c => c.User)
            .ProjectToListAsync<UserDto>(_mapper.ConfigurationProvider);
    }
}

