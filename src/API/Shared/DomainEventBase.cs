namespace Shared;

/// <summary>
/// A base type for domain events
/// </summary>
public abstract class DomainEventBase : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
