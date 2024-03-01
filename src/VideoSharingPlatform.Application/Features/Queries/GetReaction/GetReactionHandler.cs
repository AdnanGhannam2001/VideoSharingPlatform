using MediatR;

using Microsoft.AspNetCore.Identity;

using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;

using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Persistent.Data.Repositories;

namespace VideoSharingPlatform.Application.Features.Queries.GetReaction;

public record GetReactionHandler : IRequestHandler<GetReactionQuery, Result<Reaction, IEnumerable<Error>>> {
    private readonly VideosRepository _repo;
    private readonly UserManager<AppUser> _userManager;

    public GetReactionHandler(VideosRepository repo, UserManager<AppUser> userManager) {
        _repo = repo;
        _userManager = userManager;
    }

    public async Task<Result<Reaction, IEnumerable<Error>>> Handle(GetReactionQuery request, CancellationToken cancellationToken) {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null) {
            return new([new("NotFound", "User is not found")]);
        }
        
        var video = await _repo.GetByIdAsync(request.VideoId, cancellationToken);

        if (video is null) {
            return new([new("NotFound", "Video is not found")]);
        }

        Reaction? reaction = request.CommentId is null
            ? await _repo.GetReactionAsync(request.VideoId, request.UserId, cancellationToken)
            : await _repo.GetReactionOnCommentAsync(request.VideoId, request.CommentId, request.UserId, cancellationToken);

        return reaction is null ? new([new("NotFound", "Reaction is not found")]) : new(reaction);
    }
}
