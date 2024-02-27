using MediatR;

using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Persistent.Data.Repositories;

namespace VideoSharingPlatform.Application.Features.Queries.GetComments;

public class GetCommentsHandler : IRequestHandler<GetCommentsQuery, Result<IEnumerable<Comment>, IEnumerable<Error>>> {
    private readonly VideosRepository _repo;

    public GetCommentsHandler(VideosRepository repo) { _repo = repo; }

    public async Task<Result<IEnumerable<Comment>, IEnumerable<Error>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken) {
        var comments = await _repo.GetCommentsAsync(request.VideoId);

        return new(comments);
    }
}