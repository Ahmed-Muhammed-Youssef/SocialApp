namespace Shared.Specification;
/// <summary>
/// Represents a specification that defines filtering, ordering, and pagination logic
/// for a given entity type.
/// </summary>
/// <typeparam name="T">The entity type this specification applies to.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Gets the filter specification used to apply filtering criteria to a query.
    /// </summary>
    IFilterSpecification<T>? Filter { get; }

    /// <summary>
    /// Gets the maximum number of records to take (page size).
    /// </summary>
    int? Take { get; }

    /// <summary>
    /// Gets the number of records to skip before taking the next page of results.
    /// </summary>
    int? Skip { get; }

    /// <summary>
    /// Gets the lambda expression used to specify the ordering of the query.
    /// </summary>
    Func<T, object>? OrderBy { get; }

    /// <summary>
    /// Gets a value indicating whether the ordering should be descending.
    /// </summary>
    public bool IsDescending { get; }

    /// <summary>
    /// Applies pagination and ascending ordering to the specification.
    /// </summary>
    /// <param name="skip">The number of records to skip.</param>
    /// <param name="take">The maximum number of records to take.</param>
    /// <param name="orderBy">The delegate defining the ordering.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="skip"/> or <paramref name="take"/> is less than zero.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="orderBy"/> is <see langword="null"/>.
    /// </exception>
    void ApplyPagination(int skip, int take, Func<T, object> orderBy);

    /// <summary>
    /// Applies pagination and ordering (ascending or descending) to the specification.
    /// </summary>
    /// <param name="skip">The number of records to skip.</param>
    /// <param name="take">The maximum number of records to take.</param>
    /// <param name="orderBy">The delegate defining the ordering.</param>
    /// <param name="isDescending">If set to <see langword="true"/>, applies descending order; otherwise ascending.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="skip"/> or <paramref name="take"/> is less than zero.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="orderBy"/> is <see langword="null"/>.
    /// </exception>
    public void ApplyPagination(int skip, int take, Func<T, object> orderBy, bool isDescending);
}

/// <summary>
/// Represents a specification that defines filtering, ordering, pagination, and projection logic
/// for a given entity type.
/// </summary>
/// <typeparam name="T">The entity type this specification applies to.</typeparam>
/// <typeparam name="TResult">The result type produced by the projection.</typeparam>
public interface ISpecification<T, TResult> : ISpecification<T>
{
    /// <summary>
    /// Gets the selector expression used to project the entity type <typeparamref name="T"/>
    /// into a result type <typeparamref name="TResult"/>.
    /// </summary>
    Expression<Func<T, TResult>> Selectors { get; }
}
