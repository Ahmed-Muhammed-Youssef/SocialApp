namespace Shared.Specification;

/// <summary>
/// Represents a base class for filter specifications that define criteria for filtering collections of type
/// <typeparamref name="T"/>.
/// </summary>
/// <remarks>This class provides a foundation for creating filter specifications by encapsulating a filtering
/// expression. Derived classes should set the <see cref="Criteria"/> property to define specific filtering
/// logic.</remarks>
/// <typeparam name="T">The type of elements to which the filter specification applies.</typeparam>
public abstract class BaseFilterSpecification<T>(Expression<Func<T, bool>> expression) : IFilterSpecification<T>
{
    /// <inheritdoc/>
    public Expression<Func<T, bool>> Criteria { get; protected set; } = expression;
}