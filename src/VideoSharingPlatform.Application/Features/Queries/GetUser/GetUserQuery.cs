using VideoSharingPlatform.Core.Entities.AppUserAggregate;

namespace VideoSharingPlatform.Application.Features.Queries.GetUser;

public record GetUserQuery(string Id) : IQuery<AppUser>;