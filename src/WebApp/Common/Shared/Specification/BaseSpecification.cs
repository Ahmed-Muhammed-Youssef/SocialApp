using System.Linq.Expressions;

namespace Shared.Specification;

/// <summary>
/// Provides a base implementation for building specifications.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public abstract class BaseSpecification<T> : ISpecification<T>
{
    /// <inheritdoc/>
    public Expression<Func<T, bool>>? Criteria { get; protected set; }

    /// <inheritdoc/>
    public Expression<Func<T, object>>? OrderBy { get; protected set; }

    /// <inheritdoc/>
    public Expression<Func<T, object>>? OrderByDescending { get; protected set; }

    /// <inheritdoc/>
    public int? Skip { get; protected set; }

    /// <inheritdoc/>
    public int? Take { get; protected set; }

    /// <summary>
    /// Applies ascending order to the query.
    /// </summary>
    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        => OrderBy = orderByExpression;

    /// <summary>
    /// Applies descending order to the query.
    /// </summary>
    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        => OrderByDescending = orderByDescendingExpression;

    /// <summary>
    /// Applies pagination parameters.
    /// </summary>
    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }
}

/// <summary>
/// Provides a base implementation for specifications that include projection.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TResult">The type of the projected result.</typeparam>
public abstract class BaseSpecification<T, TResult> : BaseSpecification<T>, ISpecification<T, TResult>
{
    /// <inheritdoc/>
    public Expression<Func<T, TResult>>? Selector { get; protected set; }

    /// <summary>
    /// Applies a projection expression.
    /// </summary>
    protected void ApplySelector(Expression<Func<T, TResult>> selector)
        => Selector = selector;
}