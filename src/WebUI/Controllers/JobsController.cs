using MediaLink.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using MediaLink.Application.Jobs.Queries;
using MediaLink.Application.Jobs.Queries.GetJobsWithPagination;
using MediaLink.Application.Jobs.Queries.GetJob;
using MediaLink.Application.Jobs.Commands.CreateJob;
using MediaLink.Application.Jobs.Commands.DeleteJob;
using MediaLink.Application.Jobs.Commands.UpdateJob;
using Microsoft.AspNetCore.Authorization;

namespace MediaLink.WebUI.Controllers;
[Authorize]
public class JobsController : ApiControllerBase
{
    [HttpGet("{job}/Search")]
    public async Task<ActionResult<PaginatedList<JobDto>>> GetJobsWithPagination(string job)
    {
        var query = new SearchJobsWithPaginationQuery();
        query.Query = job;
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobDto>> Get(int id)
    {
        return await Mediator.Send(new GetJobQuery(id));
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateJobCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteJobCommand(id));

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateJobCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }
        await Mediator.Send(command);

        return NoContent();
    }
}
