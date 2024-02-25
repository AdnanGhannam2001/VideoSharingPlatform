using MediatR;

using Microsoft.AspNetCore.Identity;


using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;

namespace VideoSharingPlatform.Application.Features.Queries.GetUser;


public class GetUserHandler : IRequestHandler<GetUserQuery, Result<AppUser, IEnumerable<Error>>>
{
    private readonly UserManager<AppUser> _userManager;

    public GetUserHandler(UserManager<AppUser> userManager) => _userManager = userManager;

    public async Task<Result<AppUser, IEnumerable<Error>>> Handle(GetUserQuery request, CancellationToken cancellationToken) {
        var user = await _userManager.FindByIdAsync(request.Id);

        return user is not null
            ? new(user)
            : new([new("NotFound", "User is not found")]);
    }
}