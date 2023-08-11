﻿using MediaLink.WebUI.Controllers;
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
    [HttpGet("{videoId}/video")]
    public IActionResult GetVideo(string videoId)
    {
        string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "videos", videoId);

        if (System.IO.File.Exists(imagePath))
        {
            return PhysicalFile(imagePath, "video/mp4");
            //https://localhost:44447/api/Images/images/video.id/video
        }

        return NotFound();
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

        /*
                string mediaPath = Path.Combine(_hostingEnvironment.WebRootPath, "wwwroot", id);

                if (System.IO.File.Exists(mediaPath))
                {
                    string extension = Path.GetExtension(mediaPath);

                    if (extension == ".png")
                    {
                        return PhysicalFile(mediaPath, "image/png");
                    }
                    else if (extension == ".mp4")
                    {
                        return PhysicalFile(mediaPath, "video/mp4");
                    }
                    else
                    {
                        return BadRequest("Unsupported media type");
                    }
                }

                return NotFound();
         */
    }
}
