using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;
using VideoSharingPlatform.Persistent.Extensions;

namespace VideoSharingPlatform.Application.Features.Commands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<AppUser, IEnumerable<Error>>>
{
    private readonly UserManager<AppUser> _userManager;


    public CreateUserHandler(UserManager<AppUser> userManager) {
        _userManager = userManager;
    }

    public async Task<Result<AppUser, IEnumerable<Error>>> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
        var user = new AppUser(request.UserName, request.Email);

        try {
            var result = await _userManager.CreateAsync(user, request.Password);

            return result.Succeeded
                ? new(user)
                : new(result.Errors.Select(error =>
                    new Error(error.Code, "DbUpdateError", error.Description)));
        }
        catch (DbUpdateException exception) {
            return new([exception.AsError()]);
        }
    }
}