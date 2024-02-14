namespace VideoSharingPlatform.Core.Interfaces;

public interface IEntity<TKey> {
    TKey Id { get; init; }

    DateTime CreatedAtUtc { get; set; }
}

public interface IEntity : IEntity<string> { }