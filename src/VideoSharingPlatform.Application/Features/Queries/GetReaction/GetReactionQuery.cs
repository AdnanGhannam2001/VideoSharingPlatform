using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Application.Features.Queries.GetReaction;

public record GetReactionQuery(string VideoId, string? CommentId, string UserId) : IQuery<Reaction>;