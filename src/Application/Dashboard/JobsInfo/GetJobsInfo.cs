using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Application.Dashboard.PostsInfo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Dashboard.JobsInfo;
[Authorize(Roles = "Administrator")]
public record GetJobsInfo : IRequest<JobsInfoDto>
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}

public class GetJobsInfoHandler : IRequestHandler<GetJobsInfo, JobsInfoDto>
{
    private readonly IApplicationDbContext _context;

    public GetJobsInfoHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<JobsInfoDto> Handle(GetJobsInfo request, CancellationToken cancellationToken)
    {
        var dateRange = Enumerable.Range(0, 1 + request.DateTo.Subtract(request.DateFrom).Days).Select(offset => request.DateFrom.AddDays(offset)).ToList();

        var jobInfo = new JobsInfoDto
        {
            DateTimes = dateRange,
            NumberOfJobs = dateRange
                .Select(date =>
                    _context.Jobs.Count(u =>
                        u.Created.Date == date.Date
                    )
                )
                .ToList()
        };
        jobInfo.NumberOfAllJobs = await  _context.Jobs.CountAsync();
        return jobInfo;
    }
}
