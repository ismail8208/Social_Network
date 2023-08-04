using MediaLink.Application.Common.Models;
using MediaLink.Application.Projects.Commands.CreateProject;
using MediaLink.Application.Projects.Commands.DeleteProject;
using MediaLink.Application.Projects.Commands.UpdateProject;
using MediaLink.Application.Projects.Queries.GetProjectsWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaLink.WebUI.Controllers;
[Authorize]
public class ProjectsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProjectDto>>> GetProjectsWithPagination([FromQuery] GetProjectsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromForm] CreateProjectCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromForm] UpdateProjectCommand command)
    {

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteProjectCommand(id));

        return NoContent();
    }
}
