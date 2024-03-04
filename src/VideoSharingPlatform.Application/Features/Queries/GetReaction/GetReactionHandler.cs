using MediatR;
using Microsoft.AspNetCore.Identity;
using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Persistent.Data.Repositories;

namespace VideoSharingPlatform.Application.Features.Queries.GetReaction;

public record GetReactionHandler : IRequestHandler<GetReactionQuery, Result<Reaction, ExceptionBase>> {
    private readonly VideosRepository _repo;
    private readonly UserManager<AppUser> _userManager;

    public GetReactionHandler(VideosRepository repo, UserManager<AppUser> userManager) {
        _repo = repo;
        _userManager = userManager;
    }

    public async Task<Result<Reaction, ExceptionBase>> Handle(GetReactionQuery request, CancellationToken cancellationToken) {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null) {
            return new (new ExceptionBase("NotFound", "User is not found"));
        }
        
        var video = request.CommentId is null
            ? await _repo.GetWithReactionAsync(request.VideoId, user.Id, cancellationToken)
            : await _repo.GetCommentWithReactionAsync(request.VideoId, request.CommentId, user.Id, cancellationToken);

        if (video is null) {
            return new (new ExceptionBase("NotFound", "Video is not found"));
        }

        var comment = video.Comments.FirstOrDefault();

        if (comment is null && request.CommentId is not null) {
            return new (new ExceptionBase("NotFound", "Comment is not found"));
        }

        var reaction = comment is null
            ? video.Reactions.FirstOrDefault()
            : comment.Reactions.FirstOrDefault();

        return reaction is null
            ? new (new ExceptionBase("NotFound", "Reaction is not found"))
            : new (reaction);
    }
}
