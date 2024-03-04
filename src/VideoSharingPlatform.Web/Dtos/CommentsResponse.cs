using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Web.Dtos;

public record CommentsResponse(string VideoId,
    Page<Comment> Comments,
    int PageNumber,
    int PageSize,
    int Count);