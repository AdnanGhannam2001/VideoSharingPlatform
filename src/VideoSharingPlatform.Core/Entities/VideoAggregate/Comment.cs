using VideoSharingPlatform.Core.Bases;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;

namespace VideoSharingPlatform.Core.Entities.VideoAggregate;

public sealed class Comment : Entity {
    #pragma warning disable CS8618
    private Comment() { }
    #pragma warning restore CS8618

    public Comment(Video video, AppUser user, string content) : base() {
        Video = video;
        VideoId = video.Id;
        User = user;
        UserId = user.Id;
        Content = content;
        UpdatedAtUtc = CreatedAtUtc;
    }

    public string Content { get; private set; }
    
    public DateTime UpdatedAtUtc { get; private set; }

    public string VideoId { get; init; }
    public Video Video { get; init; }

    public string UserId { get; init; }
    public AppUser User { get; init; }
}
