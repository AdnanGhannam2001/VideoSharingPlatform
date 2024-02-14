namespace VideoSharingPlatform.Core.Interfaces;

public interface IAggregateRoot {
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void AddEvent(IDomainEvent @event);

    void ClearEvents();
}