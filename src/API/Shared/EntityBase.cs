namespace Shared;

/// <summary>
/// Represents the base class for entities with a unique identifier.
/// </summary>
/// <remarks>This class provides a common identifier property for derived entity classes.</remarks>
public abstract class EntityBase
{
    public int Id { get; set; }
}

public abstract class EntityBase<TId>
{
    public TId Id { get; set; } = default!;
}
