using MediatR;
using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Persistent.Data.Repositories;

namespace VideoSharingPlatform.Application.Features.Queries.GetVideos;

public class GetVideosHandler : IRequestHandler<GetVideosQuery, Result<Page<Video>, ExceptionBase>> {
    private readonly VideosRepository _repo;

    public GetVideosHandler(VideosRepository repo) { _repo = repo; }

    public async Task<Result<Page<Video>, ExceptionBase>> Handle(GetVideosQuery request, CancellationToken cancellationToken) {
        var video = await _repo.GetPageWithUserAsync(request.PageNumber,
            request.PageSize,
            x => x.CreatedAtUtc,
            true,
            cancellationToken);

        return new (video);
    }
}