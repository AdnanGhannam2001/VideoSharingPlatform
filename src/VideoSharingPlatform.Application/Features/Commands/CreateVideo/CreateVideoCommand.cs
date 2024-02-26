using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Application.Features.Commands.CreateVideo;

public record CreateVideoCommand(string UserId, string Title, string Description, string VideoExt, string ThumbnailExt)
    : ICommand<Video>;