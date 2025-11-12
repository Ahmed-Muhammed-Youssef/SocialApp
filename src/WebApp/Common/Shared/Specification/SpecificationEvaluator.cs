namespace Shared.Specification;

/// <inheritdoc/>
public class SpecificationEvaluator : ISpecificationEvaluator
{
    public static SpecificationEvaluator Default { get; } = new SpecificationEvaluator();

    /// <inheritdoc/>
    public IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification) where T : class
    {
        ArgumentNullException.ThrowIfNull(inputQuery);

        ArgumentNullException.ThrowIfNull(specification);

        IQueryable<T> query = inputQuery;

        // Apply filtering
        if (specification.Filter is not null)
        {
            query = query.Where(specification.Filter.Criteria);
        }
        
        return query;
    }

    /// <inheritdoc/>
    public IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> specification) where T : class
    {
        inputQuery = GetQuery(inputQuery, (ISpecification<T>)specification);

        return inputQuery.Select(specification.Selectors);
    }
}