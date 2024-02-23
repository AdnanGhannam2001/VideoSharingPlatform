using MediatR;

using Microsoft.AspNetCore.Identity;

using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;

namespace VideoSharingPlatform.Application.Features.Commands.ValidateUserPassword;

public class ValidateUserPasswordHandler : IRequestHandler<ValidateUserPasswordCommand, Result<AppUser, IEnumerable<Error>>>
{
    private readonly UserManager<AppUser> _userManager;

    public ValidateUserPasswordHandler(UserManager<AppUser> userManager) => _userManager = userManager;

    public async Task<Result<AppUser, IEnumerable<Error>>> Handle(ValidateUserPasswordCommand request, CancellationToken cancellationToken) {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is null) {
            return new([new(nameof(request.UserName), "User is not found")]);
        }

        var result = await _userManager.CheckPasswordAsync(user, request.Password);

        return result
            ? new(user)
            : new([new(nameof(request.Password), "Password is wrong")]);
    }
}
