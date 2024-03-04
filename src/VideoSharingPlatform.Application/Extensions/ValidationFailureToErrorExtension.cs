using FluentValidation.Results;
using VideoSharingPlatform.Core.Common;

namespace VideoSharingPlatform.Application.Extensions;

public static class ValidationFailureToErrorExtension {
    public static ExceptionBase MapToExceptionBase(this ValidationFailure validationFailure)
        => new(validationFailure.PropertyName,
            validationFailure.ErrorMessage,
            validationFailure.ErrorCode);
}