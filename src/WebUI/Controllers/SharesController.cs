using MediaLink.Application.Common.Models;
using MediaLink.Application.Shares.Commands.CreateShare;
using MediaLink.Application.Shares.Commands.DeleteShare;
using MediaLink.Application.Shares.Queries.GetSharesWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaLink.WebUI.Controllers;
[Authorize]
public class SharesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ShareDto>>> GetSharesWithPagination([FromQuery] GetSharesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateShareCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteShareCommand(id));

        return NoContent();
    }
}
