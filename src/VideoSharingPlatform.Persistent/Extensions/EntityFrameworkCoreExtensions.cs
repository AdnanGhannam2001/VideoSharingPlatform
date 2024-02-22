using Microsoft.EntityFrameworkCore;

using VideoSharingPlatform.Core.Common;

namespace VideoSharingPlatform.Persistent.Extensions;

public static class EntityFrameworkCoreExtensions {
    public static Error AsError(this DbUpdateException exception) {
        var messageText = exception.InnerException?.Data["MessageText"];

        return messageText switch {
            string m => new("", "DbUpdateError", m),
            _ => new("", "DbUpdateError", "Unknown Error")
        };
    }
}