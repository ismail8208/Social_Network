using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLink.Application.Dashboard.PostsInfo;
public class PostInfoDto
{
    public List<DateTime>? DateTimes { get; set; }
    public List<int>? NumberOfPosts { get; set; }
    public int NumberOfAllPosts { get; set; }

}
