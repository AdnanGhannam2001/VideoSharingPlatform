using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Application.Features.Queries.GetComments;

public record GetCommentsQuery(string VideoId) : IQuery<IEnumerable<Comment>>;