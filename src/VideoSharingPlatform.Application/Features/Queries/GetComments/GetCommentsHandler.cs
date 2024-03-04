using MediatR;
using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Persistent.Data.Repositories;

namespace VideoSharingPlatform.Application.Features.Queries.GetComments;

public class GetCommentsHandler : IRequestHandler<GetCommentsQuery, Result<Page<Comment>, ExceptionBase>> {
    private readonly VideosRepository _repo;

    public GetCommentsHandler(VideosRepository repo) { _repo = repo; }

    public async Task<Result<Page<Comment>, ExceptionBase>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        var video = await _repo.GetByIdAsync(request.VideoId, cancellationToken);

        if (video is null) {
            return new (new ExceptionBase("NotFound", "Video is not found"));
        }

        var page = await _repo.GetCommentsAsync(request.VideoId, request.PageNumber, request.PageSize, cancellationToken);

        return new (page);
    }
}