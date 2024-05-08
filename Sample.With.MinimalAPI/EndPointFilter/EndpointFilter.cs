namespace Sample.With.MinimalAPI.EndPointFilter;

public class EndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<EndpointFilter>();

        logger.LogInformation($"{nameof(EndpointFilter)} Invoked");

        return await next(context);
    }
}
