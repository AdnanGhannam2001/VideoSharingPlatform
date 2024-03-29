using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Application.Features.Queries.GetVideos;

public record GetVideosQuery(int PageNumber = 1, int PageSize = 10) : IQuery<IEnumerable<Video>>;