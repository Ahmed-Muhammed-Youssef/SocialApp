namespace Shared.Specification;

/// <summary>
/// Provides functionality to evaluate specifications against <see cref="IQueryable{T}"/> sources.
/// </summary>
public interface ISpecificationEvaluator
{
    /// <summary>
    /// Applies the specified filter specification to the input query.
    /// </summary>
    /// <typeparam name="T">The type of elements in the query.</typeparam>
    /// <param name="inputQuery">The initial query to which the filter will be applied. Cannot be null.</param>
    /// <param name="specification">The filter specification that defines the criteria for filtering the query. Cannot be null.</param>
    /// <returns>An <see cref="IQueryable{T}"/> representing the filtered query.</returns>
    IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, IFilterSpecification<T> specification) where T : class;
}
