namespace VideoSharingPlatform.Core.Common;

public class ExceptionBase : Exception
{
    public ExceptionBase(string propertyName, string errorMessage, string? errorCode = null) {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public required string PropertyName { get; init; }

    public required string ErrorMessage { get; init; }

    public string? ErrorCode { get; init; }

    public override string ToString()
        => $"{PropertyName}: {ErrorMessage}" + (ErrorCode is not null ? $" #{ErrorCode}" : "");
}