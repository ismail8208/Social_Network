using MediaLink.Application.Comments.Commands.DeleteComment;
using MediaLink.Application.Comments.Queries.GetCommentsWithPagination;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Likes.Commands.CreateLike;
using MediaLink.Application.Likes.Commands.DeleteLike;
using MediaLink.Application.Likes.Queries.GetLikesForJobWithPagination;
using MediaLink.Application.Likes.Queries.GetLikesWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaLink.WebUI.Controllers;
[Authorize]
public class LikesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<LikeDto>>> GetLikesWithPagination([FromQuery] GetLikesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("Job")]
    public async Task<ActionResult<PaginatedList<LikeForJobDto>>> GetLikesFoJobWithPagination([FromQuery] GetLikesForJobWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody]CreateLikeCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost("Job")]
    public async Task<ActionResult<int>> Create([FromBody] CreateLikeForJobCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteLikeCommand(id));

        return NoContent();
    }

}
