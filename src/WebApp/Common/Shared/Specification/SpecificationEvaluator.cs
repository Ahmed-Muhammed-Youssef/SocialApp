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

        if (specification.Skip is not null)
        {
            query = query.Skip(specification.Skip.Value);
        }

        if (specification.Take is not null)
        {
            query = query.Take(specification.Take.Value);
        }

        if(specification.OrderBy is not null)
        {
            if(specification.IsDescending)
            {
                query = query.OrderByDescending(specification.OrderBy).AsQueryable();
            }
            else
            {
                query = query.OrderBy(specification.OrderBy).AsQueryable();
            }
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