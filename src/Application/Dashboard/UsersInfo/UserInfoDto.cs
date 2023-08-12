using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLink.Application.Dashboard.UsersInfo;
public class UserInfoDto
{
    public List<DateTime>? DateTimes { get; set; }
    public List<int>? NumberOfUsers { get; set; }
}
