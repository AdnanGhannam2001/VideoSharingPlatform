using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Application.Features.Commands.CreateVideo;

public record CreateVideoCommand(string UserId, string Title, string Description, string Uri, string Thumbnail)
    : ICommand<Video>;