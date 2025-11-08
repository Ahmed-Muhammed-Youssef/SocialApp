namespace Shared.RepositoryBase;

/// <summary>
/// Provides a base implementation for a repository pattern, offering common data access operations for entities of type
/// <typeparamref name="T"/>.
/// </summary>
/// <remarks>This abstract class implements the <see cref="IRepositoryBase{T}"/> interface, providing asynchronous
/// methods for adding, updating, deleting, and querying entities. It utilizes a <see cref="DbContext"/> for data access
/// and an <see cref="ISpecificationEvaluator"/> to apply query specifications.</remarks>
/// <typeparam name="T">The type of the entity for which this repository is responsible. Must be a class.</typeparam>
public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected DbContext DbContext { get; set; }
    protected ISpecificationEvaluator SpecificationEvaluator { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase"/> class with the specified database context.
    /// </summary>
    /// <remarks>This constructor uses the default specification evaluator to filter and project data
    /// queries.</remarks>
    /// <param name="dbContext">The database context used to interact with the data store. Cannot be null.</param>
    public RepositoryBase(DbContext dbContext)
       : this(dbContext, Specification.SpecificationEvaluator.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase"/> class with the specified database context and
    /// specification evaluator.
    /// </summary>
    /// <param name="dbContext">The database context used to interact with the data store. Cannot be null.</param>
    /// <param name="specificationEvaluator">The specification evaluator used to apply query specifications. Cannot be null.</param>
    public RepositoryBase(DbContext dbContext, ISpecificationEvaluator specificationEvaluator)
    {
        DbContext = dbContext;
        SpecificationEvaluator = specificationEvaluator;
    }

    /// <inheritdoc/>
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<T>().Add(entity);

        await SaveChangesAsync(cancellationToken);

        return entity;
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        DbContext.Set<T>().AddRange(entities);

        await SaveChangesAsync(cancellationToken);

        return entities;
    }

    /// <inheritdoc/>
    public virtual async Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<T>().Update(entity);

        var result = await SaveChangesAsync(cancellationToken);
        return result;
    }

    /// <inheritdoc/>
    public virtual async Task<int> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        DbContext.Set<T>().UpdateRange(entities);

        var result = await SaveChangesAsync(cancellationToken);
        return result;
    }

    /// <inheritdoc/>
    public virtual async Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<T>().Remove(entity);

        var result = await SaveChangesAsync(cancellationToken);
        return result;
    }

    /// <inheritdoc/>
    public virtual async Task<int> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        DbContext.Set<T>().RemoveRange(entities);

        var result = await SaveChangesAsync(cancellationToken);
        return result;
    }

    /// <inheritdoc/>
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        return await DbContext.Set<T>().FindAsync([id], cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<T?> FirstOrDefaultAsync(IFilterSpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<List<T>> ListAsync(IFilterSpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(IFilterSpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(IFilterSpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).AnyAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().AnyAsync(cancellationToken);
    }

    /// <summary>
    /// Applies the given specification to the queryable set of entities.
    /// </summary>
    /// <param name="specification">The specification that defines the criteria and conditions to filter the entities.</param>
    /// <returns>An <see cref="IQueryable{T}"/> representing the filtered set of entities that match the specification.</returns>
    protected IQueryable<T> ApplySpecification(IFilterSpecification<T> specification)
    {
        return SpecificationEvaluator.GetQuery(DbContext.Set<T>().AsQueryable(), specification);
    }
}
