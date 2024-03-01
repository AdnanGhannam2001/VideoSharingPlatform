using VideoSharingPlatform.Core.Bases;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;

namespace VideoSharingPlatform.Core.Entities.VideoAggregate;

public sealed class Video : AggregateRoot {
    private List<Reaction> _reactions = [];
    private List<Comment> _comments = [];

    #pragma warning disable CS8618
    private Video() { }
    #pragma warning restore CS8618

    public Video(AppUser user,
        string title,
        string description,
        string videoExt,
        string thumbnailExt,
        bool hidden = false)
            : base()
    {
        User = user;
        UserId = user.Id;
        Title = title;
        Description = description;
        UpdatedAtUtc = CreatedAtUtc;
        VideoExt = videoExt;
        ThumbnailExt = thumbnailExt;
        Hidden = hidden;
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
    public string VideoExt { get; private set; }
    public string ThumbnailExt { get; private set; }
    public bool Hidden { get; private set; }

    public string UserId { get; init; }
    public AppUser User { get; init; }

    public IReadOnlyCollection<Reaction> Reactions => _reactions.AsReadOnly();
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

    public void AddComment(Comment comment) => _comments.Add(comment);

    public void AddReaction(Reaction reaction) => _reactions.Add(reaction);
}