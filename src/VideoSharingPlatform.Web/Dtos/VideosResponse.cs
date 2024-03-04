using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Web.Dtos;

public record VideosResponse(Page<Video> Videos, int PageNumber, int PageSize, int Count);