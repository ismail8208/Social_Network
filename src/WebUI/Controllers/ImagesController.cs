using MediaLink.WebUI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;
public class ImagesController : ApiControllerBase
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public ImagesController(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    [HttpGet("{id}")]
    public IActionResult GetImage(string id)
    {
        string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", id);

        if (System.IO.File.Exists(imagePath))
        {
            return PhysicalFile(imagePath, "image/png");
            //https://localhost:44447/api/Images/imageID.png 
        }

        return NotFound();
    }
}
