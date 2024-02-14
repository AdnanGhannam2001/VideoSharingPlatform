using VideoSharingPlatform.Core.Interfaces;

namespace VideoSharingPlatform.Core.Bases;

public class Entity<TKey> : IEntity<TKey>, IEquatable<Entity<TKey>>
    where TKey : IComparable
{
    public Entity(TKey id) {
        Id = id;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public TKey Id { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public static bool operator==(Entity<TKey>? entity1, Entity<TKey>? entity2) =>
        entity1 is not null && entity2 is not null && entity1.Equals(entity2);

    public static bool operator!=(Entity<TKey>? entity1, Entity<TKey>? entity2) =>
        !(entity1 == entity2);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;

        if (obj.GetType() != GetType()) return false;

        if (obj is not Entity<TKey> entity) return false;

        return entity.Id.CompareTo(Id) == 0;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public bool Equals(Entity<TKey>? other) => Equals(other);
}

public class Entity : Entity<string> {
    public Entity() : base(Guid.NewGuid().ToString()) { }
}