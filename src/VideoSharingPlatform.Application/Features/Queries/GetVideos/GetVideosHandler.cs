using MediatR;

using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Core.Interfaces;

namespace VideoSharingPlatform.Application.Features.Queries.GetVideos;

public class GetVideosHandler : IRequestHandler<GetVideosQuery, Result<IEnumerable<Video>, IEnumerable<Error>>> {
    private readonly IReadRepository<Video> _repo;

    public GetVideosHandler(IReadRepository<Video> repo) {
        _repo = repo;
    }

    public async Task<Result<IEnumerable<Video>, IEnumerable<Error>>> Handle(GetVideosQuery request, CancellationToken cancellationToken) {
        var video = await _repo.GetPageAsync(request.PageNumber,
            request.PageSize,
            x => x.CreatedAtUtc,
            true,
            cancellationToken: cancellationToken);

        return new(video);
    }
}