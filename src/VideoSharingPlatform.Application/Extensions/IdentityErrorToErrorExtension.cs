using FluentValidation.Results;

using Microsoft.AspNetCore.Identity;

using VideoSharingPlatform.Core.Common;

namespace VideoSharingPlatform.Application.Extensions;

public static class IdentityErrorToErrorExtension {
    public static ExceptionBase MapToExceptionBase(this IdentityError identityError)
        => new("IdentityError",
            identityError.Description,
            identityError.Code);

    public static ExceptionBase[] MapToExceptionBaseArray(this IEnumerable<IdentityError> identityErrors)
        => identityErrors
            .Select(MapToExceptionBase)
            .ToArray();
}