using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Web.Dtos;

public record CommentsResponse(string VideoId, IEnumerable<Comment> Comments, int PageNumber, int PageSize, int Count);