using MediaLink.Application.Common.Models;
using MediaLink.Application.Endorsements.Commands.CreateEndorsement;
using MediaLink.Application.Endorsements.Commands.DeleteEndorsement;
using MediaLink.Application.Endorsements.Queries.GetEndorsmentsWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaLink.WebUI.Controllers;
[Authorize]
public class EndorsementsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<EndorsmentDto>>> GetEndorsmentsWithPagination([FromQuery] GetEndorsmentsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateEndorsementCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteEndorsementCommand(id));

        return NoContent();
    }
}
