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
    protected RepositoryBase(DbContext dbContext)
       : this(dbContext, Specification.SpecificationEvaluator.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase"/> class with the specified database context and
    /// specification evaluator.
    /// </summary>
    /// <param name="dbContext">The database context used to interact with the data store. Cannot be null.</param>
    /// <param name="specificationEvaluator">The specification evaluator used to apply query specifications. Cannot be null.</param>
    protected RepositoryBase(DbContext dbContext, ISpecificationEvaluator specificationEvaluator)
    {
        DbContext = dbContext;
        SpecificationEvaluator = specificationEvaluator;
    }

    /// <inheritdoc/>
    public virtual T Add(T entity)
    {
        DbContext.Set<T>().Add(entity);

        return entity;
    }

    /// <inheritdoc/>
    public virtual IEnumerable<T> AddRange(IEnumerable<T> entities)
    {
        DbContext.Set<T>().AddRange(entities);

        return entities;
    }

    /// <inheritdoc/>
    public virtual void Update(T entity)
    {
        DbContext.Set<T>().Update(entity);
    }

    /// <inheritdoc/>
    public virtual void UpdateRange(IEnumerable<T> entities)
    {
        DbContext.Set<T>().UpdateRange(entities);
    }

    /// <inheritdoc/>
    public virtual void Delete(T entity)
    {
        DbContext.Set<T>().Remove(entity);
    }

    /// <inheritdoc/>
    public virtual void DeleteRange(IEnumerable<T> entities)
    {
        DbContext.Set<T>().RemoveRange(entities);
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
    public virtual async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
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
    protected IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        return SpecificationEvaluator.GetQuery(DbContext.Set<T>().AsQueryable(), specification);
    }

    /// <summary>
    /// Filters all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// <para>
    /// Projects each entity into a new form, being <typeparamref name="TResult" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <returns>The filtered projected entities as an <see cref="IQueryable{T}"/>.</returns>
    protected IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        return SpecificationEvaluator.GetQuery(DbContext.Set<T>().AsQueryable(), specification);
    }
}
