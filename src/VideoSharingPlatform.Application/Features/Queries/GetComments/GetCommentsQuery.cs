using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Application.Features.Queries.GetComments;

public record GetCommentsQuery(string VideoId, int PageNumber = 1, int PageSize = 10) : IQuery<Page<Comment>>;