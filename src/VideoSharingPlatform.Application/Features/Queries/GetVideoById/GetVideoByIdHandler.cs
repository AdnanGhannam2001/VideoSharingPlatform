using MediatR;

using VideoSharingPlatform.Core.Common;

using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Persistent.Data.Repositories;

namespace VideoSharingPlatform.Application.Features.Queries.GetVideoById;

public record GetVideoByIdHandler : IRequestHandler<GetVideoByIdQuery, Result<Video, IEnumerable<Error>>> {
    private readonly VideosRepository _repo;

    public GetVideoByIdHandler(VideosRepository repo) { _repo = repo; }

    public async Task<Result<Video, IEnumerable<Error>>> Handle(GetVideoByIdQuery request, CancellationToken cancellationToken) {
        var video = await _repo.GetByIdAsync(request.Id, cancellationToken);

        return video is null
            ? new([new("NotFound", "Video is not found")])
            : new(video);
    }
}
