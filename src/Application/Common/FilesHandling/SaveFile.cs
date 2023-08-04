using MediaLink.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace MediaLink.Application.Common.FilesHandling;
public static class SaveFile
{
    public static async Task<string> Save(FileType fileType,IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }

        var Url = "";

        if (fileType == FileType.video)
        {
            Url = "videos";
        }
        else if (fileType == FileType.image)
        {
            Url = "images";
        }
        var filename = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
        var path = Path.Combine($"wwwroot/{Url}", filename);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"wwwroot/{Url}/{filename}";
    }
}
