using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLink.Application.Dashboard.JobsInfo;
public class JobsInfoDto
{
    public List<DateTime>? DateTimes { get; set; }
    public List<int>? NumberOfJobs { get; set; }
    public int NumberOfAllJobs { get; set; }

}
