using VideoSharingPlatform.Core.Entities.AppUserAggregate;
using VideoSharingPlatform.Core.Enums;

namespace VideoSharingPlatform.Core.Entities.VideoAggregate;

public sealed class Reaction {
    #pragma warning disable CS8618
    private Reaction () { }
    #pragma warning restore CS8618

    public Reaction(Video video, AppUser user, ReactionType type) {
        Video = video;
        VideoId = video.Id;
        User = user;
        UserId = user.Id;
        Type = type;
    }

    public ReactionType Type { get; private set; }

    public string VideoId { get; init; }
    public Video Video { get; init; }

    public string UserId { get; init; }
    public AppUser User { get; init; }
}