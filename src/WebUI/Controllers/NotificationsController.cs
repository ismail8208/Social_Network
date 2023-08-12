﻿using MediaLink.WebUI.Controllers;
using Microsoft.AspNetCore.Mvc;
using MediaLink.Domain.Entities;
using MediaLink.Application.Notification;
using Microsoft.AspNetCore.Authorization;

namespace WebUI.Controllers;
[Authorize]
public class NotificationsController : ApiControllerBase
{
    [HttpGet("{userId}")]
    public async Task<ActionResult<List<Notification>>> GetNotifications(int userId)
    {
        return await Mediator.Send(new GetNotificationsQuery(userId));
    }
}