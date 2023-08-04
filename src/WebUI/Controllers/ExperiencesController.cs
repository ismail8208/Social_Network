using MediaLink.Application.Common.Models;
using MediaLink.Application.Experiences.Commands.AddProjectToExperience;
using MediaLink.Application.Experiences.Commands.CreateExperience;
using MediaLink.Application.Experiences.Commands.DeleteExperience;
using MediaLink.Application.Experiences.Commands.UpdateExperience;
using MediaLink.Application.Experiences.Queries.GetExperiencesWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaLink.WebUI.Controllers;
[Authorize]
public class ExperiencesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ExperienceDto>>> GetExperiencesWithPagination([FromQuery] GetExperiencesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateExperienceCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateExperienceCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut]
    public async Task<ActionResult> AddProjectToExperience([FromBody] AddProjectCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteExperienceCommand(id));

        return NoContent();
    }
}
