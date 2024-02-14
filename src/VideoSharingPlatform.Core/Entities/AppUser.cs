using Microsoft.AspNetCore.Identity;
using VideoSharingPlatform.Core.Interfaces;

namespace VideoSharingPlatform.Core.Entities;

public class AppUser : IdentityUser<string>, IAggregateRoot, IEntity {
    private List<IDomainEvent> _domainEvents = [];

    public AppUser() {
        Id = Guid.NewGuid().ToString();
        CreatedAtUtc = DateTime.UtcNow;
    }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public new string Id { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public void AddEvent(IDomainEvent @event) => _domainEvents.Add(@event);

    public void ClearEvents() => _domainEvents.Clear();
}