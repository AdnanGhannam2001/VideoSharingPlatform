using System.Text;

namespace VideoSharingPlatform.Core.Common;

public class ExceptionBase : Exception
{
    public ExceptionBase(string propertyName, string errorMessage, string? errorCode = null) : base() {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public string PropertyName { get; init; }

    public string ErrorMessage { get; init; }

    public string? ErrorCode { get; init; }

    public override string ToString() {
        var builder = new StringBuilder();
        if (ErrorCode is not null) {
            builder.AppendLine($"Error Code: {ErrorCode}");
        }
        builder.AppendLine($"Property: {PropertyName}");
        builder.AppendLine($"Error Message: {ErrorMessage}");

        return builder.ToString();
    }
}