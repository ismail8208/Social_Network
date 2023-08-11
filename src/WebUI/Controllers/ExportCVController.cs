using MediaLink.Application.CVService.DTOs;
using MediaLink.Application.CVService.ExportCV;
using MediaLink.Application.CVService.ReceiveCVs;
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
}
