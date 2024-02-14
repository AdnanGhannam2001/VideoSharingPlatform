using VideoSharingPlatform.Core.Interfaces;

namespace VideoSharingPlatform.Core.Bases;

public class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot {
    private List<IDomainEvent> _domainEvents = [];

    public AggregateRoot(TKey id) : base(id) { }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddEvent(IDomainEvent @event) => _domainEvents.Add(@event);

    public void ClearEvents() => _domainEvents.Clear();
}

public class AggregateRoot : AggregateRoot<string> {
    public AggregateRoot() : base(Guid.NewGuid().ToString()) { }
}