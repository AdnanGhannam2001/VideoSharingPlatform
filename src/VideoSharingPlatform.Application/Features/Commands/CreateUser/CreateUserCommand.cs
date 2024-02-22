using MediatR;
using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;

namespace VideoSharingPlatform.Application.Features.Commands.CreateUser;

public record CreateUserCommand(string UserName, string Email, string Password)
    : ICommand<AppUser>;