using VideoSharingPlatform.Core.Entities.AppUserAggregate;

namespace VideoSharingPlatform.Application.Features.Commands.UpdateUser;

public record UpdateUserCommand(string Id, string UserName, string Email, string PhoneNumber) : ICommand<AppUser>;