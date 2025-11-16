namespace Shared;

public interface IHasDomainEvents
{
    IReadOnlyCollection<DomainEventBase> DomainEvents { get; }
}
