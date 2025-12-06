namespace Shared.RepositoryBase;

/// <summary>
/// Defines a generic repository interface for performing CRUD operations and querying entities of type <typeparamref
/// name="T"/>.
/// </summary>
/// <remarks>This interface provides asynchronous methods for retrieving, adding, updating, and deleting entities.
/// It is designed to be implemented by classes that interact with a data source, such as a database.</remarks>
/// <typeparam name="T">The type of entity the repository operates on. Must be a class.</typeparam>
public interface IRepositoryBase<T> where T : class
{
    /// <summary>
    /// Finds an entity with the given primary key value.
    /// </summary>
    /// <typeparam name="TId">The type of primary key.</typeparam>
    /// <param name="id">The value of the primary key for the entity to be found.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <typeparamref name="T" />, or <see langword="null"/>.
    /// </returns>
    Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull;

    /// <summary>
    /// Asynchronously retrieves the first element that satisfies the specified filter criteria, or a default value if
    /// no such element is found.
    /// </summary>
    /// <param name="specification">The filter criteria used to evaluate elements.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first element that matches the
    /// filter criteria, or the default value for the type <typeparamref name="T"/> if no such element is found.</returns>
    Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <typeparamref name="TResult" />, or <see langword="null"/>.
    /// </returns>
    Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all entities of <typeparamref name="T" /> from the database.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
    /// </returns>
    Task<List<T>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a list of items that match the specified filter criteria.
    /// </summary>
    /// <param name="specification">The filter specification that defines the criteria for selecting items.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of items that match the
    /// filter criteria.</returns>
    Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// <para>
    /// Projects each entity into a new form, being <typeparamref name="TResult" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="List{TResult}" /> that contains elements from the input sequence.
    /// </returns>
    Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a number that represents how many entities satisfy the encapsulated query logic
    /// of the <paramref name="specification"/>.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the
    /// number of elements in the input sequence.
    /// </returns>
    Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the total number of records.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the
    /// number of elements in the input sequence.
    /// </returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether any entities satisfy the specified filter criteria asynchronously.
    /// </summary>
    /// <param name="specification">The filter specification that defines the criteria to evaluate against the entities.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if any
    /// entities satisfy the specified criteria; otherwise, <see langword="false"/>.</returns>
    Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a boolean whether any entity exists or not.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains true if the 
    /// source sequence contains any elements; otherwise, false.
    /// </returns>
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Adds an entity in the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>
    /// <typeparamref name="T" />.
    /// </returns>
    T Add(T entity);

    /// <summary>
    /// Adds the given entities in the database
    /// </summary>
    /// <param name="entities"></param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    IEnumerable<T> AddRange(IEnumerable<T> entities);

    /// <summary>
    /// Updates an entity in the database
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(T entity);

    /// <summary>
    /// Updates the given entities in the database
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    void UpdateRange(IEnumerable<T> entities);

    /// <summary>
    /// Removes an entity in the database
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(T entity);

    /// <summary>
    /// Removes the given entities in the database
    /// </summary>
    /// <param name="entities">The entities to remove.</param>
    void DeleteRange(IEnumerable<T> entities);

    /// <summary>
    /// Persists changes to the database.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
