namespace Shared.Specification;

public class Specification<T> : ISpecification<T>
{
    public IFilterSpecification<T>? Filter { get; protected set; }
}

public class Specification<T, TResult>(Expression<Func<T, TResult>> selectors) : Specification<T>
{
    public Expression<Func<T, TResult>> Selectors { get; protected set; } = selectors;
}
