using VideoSharingPlatform.Core.Bases;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;

namespace VideoSharingPlatform.Core.Entities.VideoAggregate;

public sealed class Comment : Entity {
    private List<Reaction> _reactions = [];
    private List<Comment> _replies = [];

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

    public Comment(Comment comment, AppUser user, string content) : base() {
        Parent = comment;
        ParentId = comment.Id;
        User = user;
        UserId = user.Id;
        Content = content;
        UpdatedAtUtc = CreatedAtUtc;
    }

    public string Content { get; private set; }
    
    public DateTime UpdatedAtUtc { get; private set; }

    public string? VideoId { get; init; }
    public Video? Video { get; init; }

    public string? ParentId { get; init; }
    public Comment? Parent { get; init; }

    public string UserId { get; init; }
    public AppUser User { get; init; }

    public IReadOnlyCollection<Reaction> Reactions => _reactions.AsReadOnly();
    public IReadOnlyCollection<Comment> Replies => _replies.AsReadOnly();
}
