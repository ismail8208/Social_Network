using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MediaLink.Application.Dashboard.UsersInfo;
[Authorize(Roles = "Administrator")]

public record GetUsersInfo : IRequest<UserInfoDto>
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}

public class GetUsersInfoHandler : IRequestHandler<GetUsersInfo, UserInfoDto>
{
    private readonly IApplicationDbContext _context;

    public GetUsersInfoHandler(IApplicationDbContext  context)
    {
        _context = context;
    }

    public async Task<UserInfoDto> Handle(GetUsersInfo request, CancellationToken cancellationToken)
    {
        var dateRange = Enumerable.Range(0, 1 + request.DateTo.Subtract(request.DateFrom).Days)
            .Select(offset => request.DateFrom.AddDays(offset))
            .ToList();

        var userInfo = new UserInfoDto
        {
            DateTimes = dateRange,
            NumberOfUsers = dateRange
                .Select(date =>
                    _context.InnerUsers.Count(u =>
                        u.Created.Date == date.Date
                    )
                )
                .ToList()
        };
        userInfo.NumberOfAllUsers = await _context.InnerUsers.CountAsync();
        return userInfo;
    }
}