using MediaLink.WebUI.Controllers;
using Microsoft.AspNetCore.Mvc;
using MediaLink.Domain.Entities;
using MediaLink.Application.Notification;
using Microsoft.AspNetCore.Authorization;
using MediaLink.Application.AbuseReport;

namespace WebUI.Controllers;
[Authorize]
public class NotificationsController : ApiControllerBase
{
    [HttpGet("{userId}")]
    public async Task<ActionResult<List<Notification>>> GetNotifications(int userId)
    {
        var a = await Mediator.Send(new GetNotificationsQuery(userId));
        return a;
    }

    public async Task<ActionResult> Report([FromQuery] CreateAbuseReport commad)
    {
        return Ok(await Mediator.Send(commad));
    }
}
