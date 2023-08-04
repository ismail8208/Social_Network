using MediaLink.Application.Common.Models;
using MediaLink.Application.Follows.Commands.CancelFollower;
using MediaLink.Application.Follows.Commands.Follow;
using MediaLink.Application.Follows.Commands.UnFollow;
using MediaLink.Application.Follows.Queries;
using MediaLink.Application.Follows.Queries.GetFollowers;
using MediaLink.Application.Follows.Queries.GetFollowingsWithPagination;
using MediaLink.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaLink.WebUI.Controllers;
[Authorize]
public class FollowsController : ApiControllerBase
{
    [HttpPost("Follow")]
    public async Task<ActionResult<int>> Follow([FromBody] FollowCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost("UnFollow")]
    public async Task<ActionResult<int>> UnFollow([FromBody] UnFollowCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost("CancelFollower")]
    public async Task<ActionResult<int>> CancelFollower([FromBody] CancelFollowerCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpGet("Followings")]
    public async Task<ActionResult<PaginatedList<BriefUserDto>>> GetFollowingsWithPagination([FromQuery] GetFollowingsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("Followers")]
    public async Task<ActionResult<PaginatedList<BriefUserDto>>> GetFollowersWithPagination([FromQuery] GetFollowersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }
}
