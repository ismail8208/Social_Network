using MediaLink.Application.CVService.DTOs;
using MediaLink.Application.CVService.ExportCV;
using Microsoft.AspNetCore.Mvc;

namespace MediaLink.WebUI.Controllers;
public class ExportCVController : ApiControllerBase
{
    [HttpGet("{userId}")]
    public async Task<ActionResult<CV>> ExportCV(int userId)
    {
        return await Mediator.Send(new ExportCVQuery(userId));
    }
}
