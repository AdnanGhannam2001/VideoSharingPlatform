namespace VideoSharingPlatform.Core.Entities.AppUserAggregate;

public sealed class Subscription {
    #pragma warning disable CS8618
    private Subscription () { }
    #pragma warning restore CS8618

    public Subscription(AppUser subscriber, AppUser subscribee) {
        Subscriber = subscriber;
        SubscriberId = subscriber.Id;
        Subscribee = subscribee;
        SubscribeeId = subscribee.Id;
        SubscribedAtUtc = DateTime.UtcNow;
    }
    
    public string SubscriberId { get; init; }
    public AppUser Subscriber { get; init; }
    public string SubscribeeId { get; init; }
    public AppUser Subscribee { get; init; }

    public DateTime SubscribedAtUtc { get; init; }
}