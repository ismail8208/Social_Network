﻿using MediaLink.Application.Common.Interfaces;
using MediaLink.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.CVService.ReceiveCVs;
public record ReceiveCV : IRequest<int>
{
    public int UserId { get; set; }
    public int CompanyId { get; set; }
    public int JobId { get; set; }
}
public class ReceiveCVHandler : IRequestHandler<ReceiveCV, int>
{
    private readonly IApplicationDbContext _context;

    public ReceiveCVHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(ReceiveCV request, CancellationToken cancellationToken)
    {
        var CvForAdded = new CV
        {
            Company = request.CompanyId,
            Position = request.JobId,
            UserId = request.UserId,
        };

        //Store notification start
        var user = await _context.InnerUsers.FirstOrDefaultAsync(u => u.Id == request.UserId); // مشان اسم المتقدم
        var jobName = await _context.Jobs.Include(u => u.UserId).FirstOrDefaultAsync(j => j.Id == request.JobId); // مشان عنوان الوظيفة ومعرف الشركة

        var notify = new Domain.Entities.Notification
        {
            Content = $"{user.FirstName} {user.LastName} sent his CV to work as a {jobName.Title}", // قامت شركة ما بالاعلان عن شاغر وظيفي بعنوان كذا
            DistId = jobName.UserId,
            Image = user.ProfileImage,
        };
        await _context.Notifications.AddAsync(notify);
        await _context.SaveChangesAsync(cancellationToken);
        //Store notification end

        await _context.CVs.AddAsync(CvForAdded);
        await _context.SaveChangesAsync(cancellationToken);
        return CvForAdded.CvId;
    }
}
