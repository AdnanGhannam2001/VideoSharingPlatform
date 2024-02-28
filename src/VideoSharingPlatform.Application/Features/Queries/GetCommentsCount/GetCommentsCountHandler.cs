using MediatR;

using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Persistent.Data.Repositories;

namespace VideoSharingPlatform.Application.Features.Queries.GetCommentsCount;

public class GetCommentsCountHandler : IRequestHandler<GetCommentsCountQuery, Result<int, IEnumerable<Error>>> {
    private readonly VideosRepository _repo;

    public GetCommentsCountHandler(VideosRepository repo) { _repo = repo; }

    public async Task<Result<int, IEnumerable<Error>>> Handle(GetCommentsCountQuery request, CancellationToken cancellationToken) {
        var video = await _repo.GetByIdAsync(request.VideoId, cancellationToken);

        if (video is null) {
            return new([new Error("NotFound", "Video is not found")]);
        }

        var result = await _repo.GetCommentsCountAsync(request.VideoId, cancellationToken);

        return new(result);
    }
}