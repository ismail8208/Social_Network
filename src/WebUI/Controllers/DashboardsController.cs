using MediaLink.Application.Dashboard.JobsInfo;
using MediaLink.Application.Dashboard.PostsInfo;
using MediaLink.Application.Dashboard.UsersInfo;
using MediaLink.WebUI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;
public class DashboardsController : ApiControllerBase
{
    [HttpGet("user-counts")]
    public async Task<ActionResult<UserInfoDto>> GetUserCountsAsync([FromQuery] GetUsersInfo query)
    {
        var userInfo = await Mediator.Send(query);
        return Ok(userInfo);
    }

    [HttpGet("post-counts")]
    public async Task<ActionResult<PostInfoDto>> GetPostCountsAsync([FromQuery] GetPostInfo query)
    {
        var postInfo = await Mediator.Send(query);
        return Ok(postInfo);
    }

    [HttpGet("job-counts")]
    public async Task<ActionResult<JobsInfoDto>> GetJobCountsAsync([FromQuery] GetJobsInfo query)
    {
        var jobInfo = await Mediator.Send(query);
        return Ok(jobInfo);
    }
}
