namespace Shared.Specification;

/// <summary>
/// Provides functionality to evaluate specifications against <see cref="IQueryable{T}"/> sources.
/// </summary>
public interface ISpecificationEvaluator
{
    /// <summary>
    /// Applies a projection-based specification (<see cref="ISpecification{T, TResult}"/>) to the query.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="TResult">The projected result type.</typeparam>
    /// <param name="inputQuery">The base query.</param>
    /// <param name="specification">The projection specification.</param>
    /// <returns>An <see cref="IQueryable{TResult}"/> with the specification and projection applied.</returns>
    IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> specification) where T : class;

    /// <summary>
    /// Applies the given <see cref="ISpecification{T}"/> to the provided queryable source.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="inputQuery">The base query.</param>
    /// <param name="specification">The specification defining filters, ordering, and pagination.</param>
    /// <returns>A new <see cref="IQueryable{T}"/> with the specification applied.</returns>
    IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification) where T : class;
}
