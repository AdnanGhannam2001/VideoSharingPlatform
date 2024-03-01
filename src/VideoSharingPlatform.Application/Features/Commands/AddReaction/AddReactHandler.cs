using MediatR;
using Microsoft.AspNetCore.Identity;
using VideoSharingPlatform.Application.Features.Commands.AddReaction;
using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Persistent.Data.Repositories;

namespace VideoSharingPlatform.Application.Features.Commands.AddReact;

public class AddReactionHandler : IRequestHandler<AddReactionCommand, Result<Reaction, IEnumerable<Error>>> {
    private readonly VideosRepository _repo;
    private readonly UserManager<AppUser> _userManager;

    public AddReactionHandler(VideosRepository repo, UserManager<AppUser> userManager) {
        _repo = repo;
        _userManager = userManager;
    }

    public async Task<Result<Reaction, IEnumerable<Error>>> Handle(AddReactionCommand request, CancellationToken cancellationToken) {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null) {
            return new([new("NotFound", "User is not found")]);
        }
        
        var video = await _repo.GetByIdAsync(request.VideoId, cancellationToken);

        if (video is null) {
            return new([new("NotFound", "Video is not found")]);
        }

        Reaction reaction;

        if (request.CommentId is null) {
            if (await _repo.GetReactionsCountAsync(request.VideoId, x => x.UserId == request.UserId, cancellationToken) > 0) {
                return new([new("AlreadyExists", "You've alrealy reacted to this video")]);
            }

            reaction = new(video, user, request.Type);
            video.AddReaction(reaction);
        }
        else {
            var comment = await _repo.GetCommentByIdAsync(request.VideoId, request.CommentId, cancellationToken);

            if (comment is null) {
                return new([new("NotFound", "Comment is not found")]);
            }

            if (await _repo.GetReactionsOnCommentCountAsync(request.VideoId,
                request.CommentId, x => x.UserId == request.UserId, cancellationToken) > 0)
            {
                return new([new("AlreadyExists", "You've alrealy reacted to this comment")]);
            }

            reaction = new(video, user, request.Type);
            comment.AddReaction(reaction);
        }

        await _repo.SaveChangesAsync(cancellationToken);

        return new(reaction);
    }
}
