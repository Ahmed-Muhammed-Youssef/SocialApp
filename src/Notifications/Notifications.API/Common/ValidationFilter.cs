namespace Notifications.API.Common;

/// <summary>Validates the endpoint argument of type <typeparamref name="T"/> and short-circuits with a 400 validation problem when it is invalid.</summary>
public sealed class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        T argument = context.Arguments.OfType<T>().Single();

        var result = await validator.ValidateAsync(argument, context.HttpContext.RequestAborted);
        if (!result.IsValid)
        {
            return TypedResults.ValidationProblem(result.ToDictionary());
        }

        return await next(context);
    }
}
