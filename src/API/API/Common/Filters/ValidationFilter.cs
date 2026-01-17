using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Common.Filters;

public sealed class ValidationFilter(IServiceProvider serviceProvider, ProblemDetailsFactory problemDetailsFactory) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionArguments.Count == 0)
        {
            await next();
            return;
        }

        var failures = new List<FluentValidation.Results.ValidationFailure>();

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

            if (serviceProvider.GetService(validatorType) is not IValidator validator)
            {
                continue;
            }

            var validationContext = new ValidationContext<object>(argument);
            var result = await validator.ValidateAsync(validationContext);

            if (!result.IsValid)
            {
                failures.AddRange(result.Errors);
            }
        }

        if (failures.Count == 0)
        {
            await next();
            return;
        }

        var modelState = new ModelStateDictionary();

        foreach (var failure in failures)
        {
            modelState.AddModelError(
                failure.PropertyName ?? string.Empty,
                failure.ErrorMessage);
        }

        var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
            context.HttpContext,
            modelState,
            StatusCodes.Status400BadRequest);

        context.Result = new BadRequestObjectResult(problemDetails);
    }
}
