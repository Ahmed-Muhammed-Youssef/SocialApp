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
    /// Gets the collection of navigation property expressions to include in query results.
    /// </summary>
    /// <remarks>Use this property to specify related entities that should be eagerly loaded as part of the
    /// query. Each expression identifies a navigation property to include. This is commonly used in data access
    /// scenarios to reduce the number of database queries by retrieving related data in a single operation.</remarks>
    List<Expression<Func<T, object>>> Includes { get; }

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
    bool IsDescending { get; }

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
    void ApplyPagination(int skip, int take, Func<T, object> orderBy, bool isDescending);

    /// <summary>
    /// Specifies a related entity or navigation property to include in the query results.
    /// </summary>
    /// <remarks>Use this method to eagerly load related data as part of the query. Multiple calls to this
    /// method can be used to include multiple related entities.</remarks>
    /// <param name="includeExpression">An expression that identifies the related entity or navigation property to include. Typically a lambda
    /// expression such as 'entity => entity.Property'. Cannot be null.</param>
    void AddInclude(Expression<Func<T, object>> includeExpression);
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
