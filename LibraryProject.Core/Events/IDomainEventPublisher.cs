namespace Core.Events;

public interface IDomainEventPublisher
{
    Task PublishEventsAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
