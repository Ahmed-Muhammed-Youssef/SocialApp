namespace Shared.Specification;

/// <summary>
/// Defines the basic contract for a specification that filters, orders, and paginates entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the entity this specification applies to.</typeparam>
public interface IFilterSpecification<T>
{
    /// <summary>
    /// Gets the filter criteria used to select matching entities.
    /// </summary>
    Expression<Func<T, bool>> Criteria { get; }
}
