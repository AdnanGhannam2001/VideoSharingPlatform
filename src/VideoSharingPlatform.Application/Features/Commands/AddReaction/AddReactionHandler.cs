using MediatR;
using Microsoft.AspNetCore.Identity;
using VideoSharingPlatform.Application.Features.Commands.AddReaction;
using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Persistent.Data.Repositories;

namespace VideoSharingPlatform.Application.Features.Commands.AddReact;

public class AddReactionHandler : IRequestHandler<AddReactionCommand, Result<Reaction, ExceptionBase>> {
    private readonly VideosRepository _repo;
    private readonly UserManager<AppUser> _userManager;

    public AddReactionHandler(VideosRepository repo, UserManager<AppUser> userManager) {
        _repo = repo;
        _userManager = userManager;
    }

    public async Task<Result<Reaction, ExceptionBase>> Handle(AddReactionCommand request, CancellationToken cancellationToken) {
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

        Reaction? reaction;

        if (request.CommentId is null) {
            reaction = video.Reactions.FirstOrDefault();

            if (reaction is not null) {
                reaction.ChangeType(request.Type);
            } else {
                reaction = new(video, user, request.Type);
                video.AddReaction(reaction);
            }
        }
        else {
            var comment = video.Comments.FirstOrDefault();

            if (comment is null) {
                return new (new ExceptionBase("NotFound", "Comment is not found"));
            }

            reaction = comment.Reactions.FirstOrDefault();

            if (reaction is not null) {
                reaction.ChangeType(request.Type);
            } else {
                reaction = new(video, user, request.Type);
                comment.AddReaction(reaction);
            }
        }

        await _repo.SaveChangesAsync(cancellationToken);

        return new (reaction);
    }
}
