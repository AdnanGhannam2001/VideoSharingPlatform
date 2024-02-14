using VideoSharingPlatform.Core.Bases;

namespace VideoSharingPlatform.Core.Entities.AppUserAggregate;

public sealed class Notification : Entity {
    #pragma warning disable CS8618
    private Notification() { }
    #pragma warning restore CS8618

    public Notification(AppUser user, string content) : base() {
        User = user;
        UserId = user.Id;
        Content = content;
        Read = false;
    }

    public string Content { get; private set; }

    public bool Read { get; private set; }

    public string UserId { get; init; }
    public AppUser User { get; init; }
}