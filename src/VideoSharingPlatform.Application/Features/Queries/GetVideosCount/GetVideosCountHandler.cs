using MediatR;

using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Core.Interfaces;

namespace VideoSharingPlatform.Application.Features.Queries.GetVideosCount;

public record GetVideosCountHandler : IRequestHandler<GetVideosCountQuery, Result<int, IEnumerable<Error>>> {
    private readonly IReadRepository<Video> _repo;

    public GetVideosCountHandler(IReadRepository<Video> repo) { _repo = repo; }
    
    public async Task<Result<int, IEnumerable<Error>>> Handle(GetVideosCountQuery request, CancellationToken cancellationToken) {
        var count = await _repo.CountAsync(cancellationToken);

        return new(count);
    }
}
