using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Application.Follows.Queries.GetFollowers;
using MediaLink.Application.Notification;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.AbuseReport;

public record CreateAbuseReport : IRequest
{
    public int ReporterId { get; set; }
    public int AbuserId { get; set; }
}
public class CreateAbuseReportHandler : IRequestHandler<CreateAbuseReport>
{
    private readonly IApplicationDbContext _context;

    public CreateAbuseReportHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(CreateAbuseReport request, CancellationToken cancellationToken)
    {

        var Reporter = await _context.InnerUsers.FirstOrDefaultAsync(u => u.Id == request.ReporterId); // الشخص الي قام بالبلاغ
        var Abuser = await _context.InnerUsers.FirstOrDefaultAsync(u => u.Id == request.AbuserId); // الشخص المسيئ
        var Admins = await _context.InnerUsers.Where(u => u.specialization == "Admin").ToListAsync(); // المسؤوليين

        foreach (var Ad in Admins)
        {
            var not = new Domain.Entities.Notification
            {
                DistId = Ad.Id, //اي دي الادمن
                Content = $"{Reporter.FirstName} {Reporter.LastName} reported on .. {Abuser.UserName}({Abuser.FirstName} {Abuser.LastName})", // قام اسماعيل بالابلاغ عن احمد
                Image = Reporter.ProfileImage, // صورة المبلغ
            };
            await _context.Notifications.AddAsync(not);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return Unit.Value;
    }
}