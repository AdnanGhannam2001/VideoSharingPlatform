using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Application.Features.Queries.GetVideoById;

public record GetVideoByIdQuery(string Id) : IQuery<Video>;