using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Web.Dtos;

public record VideosResponse(IEnumerable<Video> Videos, int PageNumber, int PageSize, int Count);