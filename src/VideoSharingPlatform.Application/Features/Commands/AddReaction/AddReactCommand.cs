using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Core.Enums;

namespace VideoSharingPlatform.Application.Features.Commands.AddReaction;

public record AddReactionCommand(string VideoId, string? CommentId, string UserId, ReactionType Type) : ICommand<Reaction>;