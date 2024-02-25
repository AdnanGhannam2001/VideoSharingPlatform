using MediatR;
using VideoSharingPlatform.Application.Features.Queries.GetUser;
using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Core.Interfaces;

namespace VideoSharingPlatform.Application.Features.Commands.CreateVideo;


public class CreateVideoHandler : IRequestHandler<CreateVideoCommand, Result<Video, IEnumerable<Error>>> {
    private readonly IMediator _mediator;
    private readonly IRepository<Video> _repo;

    public CreateVideoHandler(IMediator mediator, IRepository<Video> repo) {
        _mediator = mediator;
        _repo = repo;
    }

    public async Task<Result<Video, IEnumerable<Error>>> Handle(CreateVideoCommand request, CancellationToken cancellationToken) {
        var user = await _mediator.Send(new GetUserQuery(request.UserId), cancellationToken);

        if (!user.IsSuccess) {
            return new(user.Error!);
        }

        var video = new Video(user.Value!, request.Title, request.Description);

        await _repo.AddAsync(video, cancellationToken);

        await _repo.SaveChangesAsync(cancellationToken);

        return new(video);
    }
}