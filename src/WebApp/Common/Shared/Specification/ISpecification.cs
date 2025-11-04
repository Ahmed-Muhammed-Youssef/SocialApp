using System.Linq.Expressions;

namespace Shared.Specification;

/// <summary>
/// Defines the basic contract for a specification that filters, orders, and paginates entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the entity this specification applies to.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Gets the filter criteria used to select matching entities.
    /// </summary>
    Expression<Func<T, bool>>? Criteria { get; }

    /// <summary>
    /// Gets the expression used to order the query results ascendingly.
    /// </summary>
    Expression<Func<T, object>>? OrderBy { get; }

    /// <summary>
    /// Gets the expression used to order the query results descendingly.
    /// </summary>
    Expression<Func<T, object>>? OrderByDescending { get; }

    /// <summary>
    /// Gets the number of items to skip for pagination.
    /// </summary>
    int? Skip { get; }

    /// <summary>
    /// Gets the number of items to take for pagination.
    /// </summary>
    int? Take { get; }
}

/// <summary>
/// Defines the contract for a specification that includes a projection
/// from <typeparamref name="T"/> to <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the entity this specification applies to.</typeparam>
/// <typeparam name="TResult">The type of the projected result.</typeparam>
public interface ISpecification<T, TResult> : ISpecification<T>
{
    /// <summary>
    /// Gets the projection expression that maps from <typeparamref name="T"/> to <typeparamref name="TResult"/>.
    /// </summary>
    Expression<Func<T, TResult>>? Selector { get; }
}
