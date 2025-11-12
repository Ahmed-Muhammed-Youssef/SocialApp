namespace Shared.Specification;

public interface ISpecification<T>
{
    IFilterSpecification<T>? Filter { get; }
}

public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T, TResult>> Selectors { get; }
}
