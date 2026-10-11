namespace Notifications.API.Common;

/// <summary>Runs the FluentValidation validator for <typeparamref name="TRequest"/> and returns a 400 ValidationProblem on failure.</summary>
public sealed class ValidationFilter<TRequest>(IValidator<TRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        TRequest? request = context.Arguments.OfType<TRequest>().FirstOrDefault();
        if (request is null)
        {
            return TypedResults.BadRequest();
        }

        FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);
        if (!result.IsValid)
        {
            return TypedResults.ValidationProblem(result.ToDictionary());
        }

        return await next(context);
    }
}
