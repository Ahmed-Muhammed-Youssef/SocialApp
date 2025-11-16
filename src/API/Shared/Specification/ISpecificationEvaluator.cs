namespace Shared.Specification;

/// <summary>
/// Provides functionality to evaluate specifications against <see cref="IQueryable{T}"/> sources.
/// </summary>
public interface ISpecificationEvaluator
{
    /// <summary>
    /// Applies the specified specification to the input query.
    /// </summary>
    /// <typeparam name="T">The type of elements in the query.</typeparam>
    /// <param name="inputQuery">The initial query to which the filter will be applied. Cannot be null.</param>
    /// <param name="specification">The specification that defines the criteria for the query. Cannot be null.</param>
    /// <returns>An <see cref="IQueryable{T}"/> representing the filtered query.</returns>
    IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification) where T : class;

    /// <summary>
    /// Applies the specified specification to the input query.
    /// </summary>
    /// <typeparam name="T">The type of elements in the query.</typeparam>
    /// <param name="inputQuery">The initial query to which the filter will be applied. Cannot be null.</param>
    /// <param name="specification">The specification that defines the criteria for the query. Cannot be null.</param>
    /// <returns>An <see cref="IQueryable{TResult}"/> representing the filtered query.</returns>
    IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> specification) where T : class;
}
