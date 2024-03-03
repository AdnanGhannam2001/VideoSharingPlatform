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

        Reaction? reaction = new(video, user, request.Type);

        if (request.CommentId is null) {
            reaction = await _repo.GetReactionAsync(video.Id, user.Id, cancellationToken);

            if (reaction is not null) {
                reaction.ChangeType(request.Type);
            } else {
                reaction = new(video, user, request.Type);
                video.AddReaction(reaction);
            }
        }
        else {
            var comment = await _repo.GetCommentByIdAsync(request.VideoId, request.CommentId, cancellationToken);

            if (comment is null) {
                return new([new("NotFound", "Comment is not found")]);
            }

            reaction = await _repo.GetReactionOnCommentAsync(video.Id, comment.Id, user.Id, cancellationToken);

            if (reaction is not null) {
                reaction.ChangeType(request.Type);
            } else {
                reaction = new(video, user, request.Type);
                comment.AddReaction(reaction);
            }
        }

        await _repo.SaveChangesAsync(cancellationToken);

        return new(reaction);
    }
}
