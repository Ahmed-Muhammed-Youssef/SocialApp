namespace Shared.Specification;

/// <inheritdoc/>
public class Specification<T> : ISpecification<T>
{
    /// <inheritdoc/>
    public IFilterSpecification<T>? Filter { get; protected set; }

    /// <inheritdoc/>
    public int? Take { get; private set; }

    /// <inheritdoc/>
    public int? Skip { get; private set; }

    /// <inheritdoc/>
    public Func<T, object>? OrderBy { get; private set; }

    /// <inheritdoc/>
    public bool IsDescending { get; private set; }

    /// <inheritdoc/>
    public void ApplyPagination(int skip, int take, Func<T, object> orderBy) => ApplyPagination(skip, take, orderBy, isDescending: false);

    /// <inheritdoc/>
    public void ApplyPagination(int skip, int take, Func<T, object> orderBy, bool isDescending)
    {
        if (skip < 0)
            throw new ArgumentOutOfRangeException(nameof(skip), "Skip value cannot be negative.");

        if (take < 0)
            throw new ArgumentOutOfRangeException(nameof(take), "Take value cannot be negative.");

        OrderBy = orderBy ?? throw new ArgumentNullException(nameof(orderBy));
        Skip = skip;
        Take = take;
        IsDescending = isDescending;
    }
}

/// <inheritdoc/>
public class Specification<T, TResult>(Expression<Func<T, TResult>> selectors) : Specification<T>
{
    /// <inheritdoc/>
    public Expression<Func<T, TResult>> Selectors { get; protected set; } = selectors;
}
