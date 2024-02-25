using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Application.Features.Commands.CreateVideo;

public record CreateVideoCommand(string UserId, string Title, string Description)
    : ICommand<Video>;