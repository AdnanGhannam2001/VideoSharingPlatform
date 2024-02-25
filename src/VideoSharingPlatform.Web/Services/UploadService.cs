using VideoSharingPlatform.Core.Interfaces;

namespace VideoSharingPlatform.Web.Services;

internal class UploadService : IUploadService<IFormFile> {
    private async Task UploadFileAsync(IFormFile file, string path, string name) {
        var ext = Path.GetExtension(file.FileName);
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);

        if (!Directory.Exists(fullPath)) {
            Directory.CreateDirectory(fullPath);
        }

        var fileName = Path.Combine(fullPath, name + ext);

        using var fileStream = new FileStream(fileName, FileMode.Create);

        await file.CopyToAsync(fileStream);
    }

    public Task UploadVideoAsync(IFormFile video, string name) => UploadFileAsync(video, "videos", name);

    public Task UploadImageAsync(IFormFile image, string name) => UploadFileAsync(image, "thumbnails", name);
}