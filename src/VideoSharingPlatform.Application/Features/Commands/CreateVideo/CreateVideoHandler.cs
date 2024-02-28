using MediatR;
using Microsoft.AspNetCore.Http;
using VideoSharingPlatform.Application.Features.Queries.GetUser;
using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Core.Interfaces;

namespace VideoSharingPlatform.Application.Features.Commands.CreateVideo;

public class CreateVideoHandler : IRequestHandler<CreateVideoCommand, Result<Video, IEnumerable<Error>>> {
    private readonly IMediator _mediator;
    private readonly IRepository<Video> _repo;
    private readonly IUploadService<IFormFile> _uploadService;

    public CreateVideoHandler(IMediator mediator, IRepository<Video> repo, IUploadService<IFormFile> uploadService) {
        _mediator = mediator;
        _repo = repo;
        _uploadService = uploadService;
    }

    public async Task<Result<Video, IEnumerable<Error>>> Handle(CreateVideoCommand request, CancellationToken cancellationToken) {
        var user = await _mediator.Send(new GetUserQuery(request.UserId), cancellationToken);

        if (!user.IsSuccess) {
            return new(user.Error!);
        }

        var video = new Video(user.Value!,
            request.Title,
            request.Description,
            Path.GetExtension(request.VideoFile.FileName),
            Path.GetExtension(request.Thumbnail.FileName));

        await using var transation = await _repo.BeginTransactionAsync(cancellationToken);

        try {
            await _repo.AddAsync(video, cancellationToken);
            await _repo.SaveChangesAsync(cancellationToken);

            await _uploadService.UploadVideoAsync(request.VideoFile, request.Path, video.Id);
            await _uploadService.UploadImageAsync(request.Thumbnail, request.Path, video.Id);

            await transation.CommitAsync(cancellationToken);
        }
        catch (Exception) {
            await transation.RollbackAsync(cancellationToken);
        }

        return new(video);
    }
}