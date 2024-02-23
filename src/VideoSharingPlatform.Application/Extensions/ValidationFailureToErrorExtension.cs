using FluentValidation.Results;

using VideoSharingPlatform.Core.Common;

namespace VideoSharingPlatform.Application.Extensions;

public static class ValidationFailureToErrorExtension {
    public static Error MapToError(this ValidationFailure validationFailure)
        => new(
            validationFailure.PropertyName,
            validationFailure.ErrorMessage,
            validationFailure.ErrorCode);
}