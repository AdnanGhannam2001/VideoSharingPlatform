using Microsoft.AspNetCore.Identity;
using NanoidDotNet;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Core.Interfaces;

namespace VideoSharingPlatform.Core.Entities.AppUserAggregate;

public sealed class AppUser : IdentityUser<string>, IAggregateRoot {
    private List<IDomainEvent> _domainEvents = [];
    private List<Notification> _notifications = [];
    private List<Video> _videos = [];
    private List<Reaction> _reactions = [];
    private List<Comment> _comments = [];
    private List<Subscription> _subscribers = [];
    private List<Subscription> _subscribedTo = [];

    private AppUser() { }

    public AppUser(string username, string email) : base() {
        Id = Nanoid.Generate();
        UserName = username;
        Email = email;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public DateTime CreatedAtUtc { get; init; }

    public void AddEvent(IDomainEvent @event) => _domainEvents.Add(@event);

    public void ClearEvents() => _domainEvents.Clear();

    public IReadOnlyCollection<Notification> Notifications => _notifications.AsReadOnly();
    public IReadOnlyCollection<Video> Videos => _videos.AsReadOnly();
    public IReadOnlyCollection<Reaction> Reactions => _reactions.AsReadOnly();
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
    public IReadOnlyCollection<Subscription> Subscribers => _subscribers.AsReadOnly();
    public IReadOnlyCollection<Subscription> SubscribedTo => _subscribedTo.AsReadOnly();
}