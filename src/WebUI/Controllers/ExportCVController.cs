using MediaLink.Application.CVService.DTOs;
using MediaLink.Application.CVService.ExportCV;
using MediaLink.Application.CVService.GetCVs;
using MediaLink.Application.CVService.ReceiveCVs;
using MediaLink.Application.Users.Queries.FindUser;
using Microsoft.AspNetCore.Mvc;

namespace MediaLink.WebUI.Controllers;
public class ExportCVController : ApiControllerBase
{
    [HttpGet("{userId}")]
    public async Task<ActionResult<CV>> ExportCV(int userId)
    {
        return await Mediator.Send(new ExportCVQuery(userId));
    }

    [HttpPost]
    public async Task<ActionResult<int>> ReveiveCV(ReceiveCV command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("{jobId}/jobs")]
    public async Task<ActionResult<List<UserDto>>?> GetCVs(int jobId)
    {
        return await Mediator.Send(new GetCVsQuery(jobId));
    }
}
