using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Application.Features.Commands.AddComment;

public record AddCommentCommand(string VideoId, string UserId, string Content, bool IsReply = false) : ICommand<Comment>;