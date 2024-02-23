namespace VideoSharingPlatform.Core.Common;

public record Error(string PropertyName, string ErrorMessage, string? ErrorCode = null);