namespace Notifications.API.Features.Push;

public static class PushEndpoint
{
    public const string Route = "notifications/push";

    public static IEndpointRouteBuilder MapPushEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(Route, HandleAsync)
            .AddEndpointFilter<ApiKeyEndpointFilter>()
            .AddEndpointFilter<ValidationFilter<PushRequest>>()
            .WithName("push");

        return app;
    }

    /// <summary>
    /// Always 202 for a valid request: the notification is stored and will be delivered asynchronously.
    /// A duplicate EventId is also 202, so producers can safely retry.
    /// </summary>
    private static async Task<Accepted<PushResponse>> HandleAsync(PushRequest request, PushHandler handler, CancellationToken cancellationToken)
    {
        bool accepted = await handler.HandleAsync(request, cancellationToken);
        return TypedResults.Accepted((string?)null, new PushResponse(request.EventId, Duplicate: !accepted));
    }
}
