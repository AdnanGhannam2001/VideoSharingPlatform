using VideoSharingPlatform.Core.Entities.AppUserAggregate;

namespace VideoSharingPlatform.Application.Features.Commands.ValidateUserPassword;

public record ValidateUserPasswordCommand(string UserName, string Password) : ICommand<AppUser>;