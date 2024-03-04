using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;
using VideoSharingPlatform.Persistent.Extensions;

namespace VideoSharingPlatform.Application.Features.Commands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<AppUser, ExceptionBase>>
{
    private readonly UserManager<AppUser> _userManager;


    public CreateUserHandler(UserManager<AppUser> userManager) {
        _userManager = userManager;
    }

    public async Task<Result<AppUser, ExceptionBase>> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
        var user = new AppUser(request.UserName, request.Email);

        try {
            var result = await _userManager.CreateAsync(user, request.Password);

            return result.Succeeded
                ? new(user)
                : new(result.Errors.Select(error =>
                    new ExceptionBase("DbUpdateError", error.Description, error.Code))
                    .ToArray());
        }
        catch (DbUpdateException exception) {
            return new (exception.AsError());
        }
    }
}