using VideoSharingPlatform.Core.Interfaces;

namespace VideoSharingPlatform.Core.Bases;

public class Entity<TKey> : IEntity<TKey> {
    public Entity(TKey id) {
        Id = id;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public TKey Id { get; init; }

    public DateTime CreatedAtUtc { get; init; }
}

public class Entity : Entity<string> {
    public Entity() : base(Guid.NewGuid().ToString()) { }
}