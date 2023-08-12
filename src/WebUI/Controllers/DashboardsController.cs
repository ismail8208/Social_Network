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
}
