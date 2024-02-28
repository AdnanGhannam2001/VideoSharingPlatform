using Microsoft.AspNetCore.Http;

using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Application.Features.Commands.CreateVideo;

public record CreateVideoCommand(string UserId,
    string Title,
    string Description,
    string Path,
    IFormFile VideoFile,
    IFormFile Thumbnail)
        : ICommand<Video>;