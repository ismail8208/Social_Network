using MediaLink.Application.Common.Models;
using MediaLink.Application.Educations.Commands.CreateEducation;
using MediaLink.Application.Educations.Commands.DeleteEducation;
using MediaLink.Application.Educations.Commands.UpdateEducation;
using MediaLink.Application.Educations.Queries;
using MediaLink.Application.Educations.Queries.GetEducationsWithPagination;
using MediaLink.Application.Educations.Queries.SearchEducation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaLink.WebUI.Controllers;
[Authorize]
public class EducationsController : ApiControllerBase
{
    [HttpGet("{education}/Search")]
    public async Task<ActionResult<PaginatedList<EducationDto>>> Search(string education)
    {
        var query = new SearchEducationQuery();
        query.Query = education;
        return await Mediator.Send(query);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<EducationDto>>> GetEducationsWithPagination([FromQuery] GetEducationsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateEducationCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateEducationCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteEducationCommand(id));

        return NoContent();
    }
}
