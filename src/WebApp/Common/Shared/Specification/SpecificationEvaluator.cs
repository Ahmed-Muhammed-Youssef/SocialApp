
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
        if (specification.Criteria is not null)
            query = query.Where(specification.Criteria);

        // Apply ordering
        if (specification.OrderBy is not null)
            query = query.OrderBy(specification.OrderBy);
        else if (specification.OrderByDescending is not null)
            query = query.OrderByDescending(specification.OrderByDescending);

        // Apply pagination
        if (specification.Skip.HasValue && specification.Take.HasValue)
        {
            query = query.Skip(specification.Skip.Value);
            query = query.Take(specification.Take.Value);
        }

        return query;
    }

    /// <inheritdoc/>
    public IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> specification) where T : class
    {
        ArgumentNullException.ThrowIfNull(inputQuery);

        ArgumentNullException.ThrowIfNull(specification);

        // Apply base specification logic
        var query = GetQuery(inputQuery, (ISpecification<T>)specification);

        // Apply projection (Selector)
        if (specification.Selector is null)
        {
            throw new InvalidOperationException("Specification with projection must define a Selector.");
        }

        return query.Select(specification.Selector);
    }
}