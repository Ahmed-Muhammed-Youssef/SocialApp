namespace Shared.Specification;

/// <inheritdoc/>
public class SpecificationEvaluator : ISpecificationEvaluator
{
    public static SpecificationEvaluator Default { get; } = new SpecificationEvaluator();

    /// <inheritdoc/>
    public IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, IFilterSpecification<T> specification) where T : class
    {
        ArgumentNullException.ThrowIfNull(inputQuery);

        ArgumentNullException.ThrowIfNull(specification);

        IQueryable<T> query = inputQuery;

        // Apply filtering
        if (specification.Criteria is not null)
            query = query.Where(specification.Criteria);

        return query;
    }
}