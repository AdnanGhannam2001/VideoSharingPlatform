using MediatR;
using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Persistent.Data.Repositories;

namespace VideoSharingPlatform.Application.Features.Queries.GetVideoById;

public record GetVideoByIdHandler : IRequestHandler<GetVideoByIdQuery, Result<Video, ExceptionBase>> {
    private readonly VideosRepository _repo;

    public GetVideoByIdHandler(VideosRepository repo) { _repo = repo; }

    public async Task<Result<Video, ExceptionBase>> Handle(GetVideoByIdQuery request, CancellationToken cancellationToken) {
        var video = await _repo.GetByIdAsync(request.Id, cancellationToken);

        return video is null
            ? new (new ExceptionBase("NotFound", "Video is not found"))
            : new (video);
    }
}
