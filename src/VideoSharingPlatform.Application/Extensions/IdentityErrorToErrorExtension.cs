using FluentValidation.Results;

using Microsoft.AspNetCore.Identity;

using VideoSharingPlatform.Core.Common;

namespace VideoSharingPlatform.Application.Extensions;

public static class IdentityErrorToErrorExtension {
    public static Error MapToError(this IdentityError identityError)
        => new("IdentityError",
            identityError.Description,
            identityError.Code);

    public static IEnumerable<Error> MapToErrors(this IEnumerable<IdentityError> identityErrors)
        => identityErrors.Select(error => error.MapToError());
}