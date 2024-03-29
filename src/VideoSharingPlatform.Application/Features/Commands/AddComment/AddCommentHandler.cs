using MediatR;
using VideoSharingPlatform.Application.Features.Queries.GetUser;
using VideoSharingPlatform.Application.Features.Queries.GetVideoById;
using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Persistent.Data.Repositories;

namespace VideoSharingPlatform.Application.Features.Commands.AddComment;

public class AddCommentHandler : IRequestHandler<AddCommentCommand, Result<Comment, IEnumerable<Error>>> {
    private readonly VideosRepository _repo;
    private readonly IMediator _mediator;

    public AddCommentHandler(VideosRepository repo, IMediator mediator)
    {
        _repo = repo;
        _mediator = mediator;
    }

    public async Task<Result<Comment, IEnumerable<Error>>> Handle(AddCommentCommand request, CancellationToken cancellationToken) {
        var video = await _repo.GetByIdAsync(request.VideoId, cancellationToken);

        if (video is null) {
            return new([new ("NotFound", "Video is not found")]);
        }

        var userResult = await _mediator.Send(new GetUserQuery(request.UserId), cancellationToken);

        if (!userResult.IsSuccess) {
            return new([new ("NotFound", "User is not found")]);
        }

        var comment = new Comment(video, userResult.Value!, request.Content);
         
        video.AddComment(comment);

        await _repo.SaveChangesAsync(cancellationToken);

        return new(comment);
    }
}