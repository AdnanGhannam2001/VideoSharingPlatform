using Microsoft.AspNetCore.Http;

using VideoSharingPlatform.Core.Interfaces;

namespace VideoSharingPlatform.Application.Services;

public class UploadService : IUploadService<IFormFile> {
    private static async Task UploadFileAsync(IFormFile file, string path, string name) {
        var ext = Path.GetExtension(file.FileName);

        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }

        var fileName = Path.Combine(path, name + ext);

        using var fileStream = new FileStream(fileName, FileMode.Create);

        await file.CopyToAsync(fileStream);
    }

    public Task UploadVideoAsync(IFormFile video, string path, string name)
        => UploadFileAsync(video, Path.Combine(path, "videos"), name);

    public Task UploadImageAsync(IFormFile image, string path, string name)
        => UploadFileAsync(image, Path.Combine(path, "thumbnails"), name);
}