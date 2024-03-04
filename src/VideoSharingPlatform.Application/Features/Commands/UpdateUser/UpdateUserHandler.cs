using MediatR;
using Microsoft.AspNetCore.Identity;
using VideoSharingPlatform.Application.Extensions;
using VideoSharingPlatform.Application.Features.Queries.GetUser;
using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;

namespace VideoSharingPlatform.Application.Features.Commands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<AppUser, ExceptionBase>> {
    private readonly UserManager<AppUser> _userManager;
    private readonly IMediator _mediator;

    public UpdateUserHandler(UserManager<AppUser> userManager, IMediator mediator) {
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<Result<AppUser, ExceptionBase>> Handle(UpdateUserCommand request, CancellationToken cancellationToken) {
        var userResult = await _mediator.Send(new GetUserQuery(request.Id), cancellationToken);

        if (!userResult.IsSuccess) {
            return new (userResult.Exceptions!);
        }

        var user = userResult.Value!;

        user.Update(request.UserName, request.Email, request.PhoneNumber);

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? new (userResult.Value!)
            : new (result.Errors.MapToExceptionBaseArray());
    }
}
