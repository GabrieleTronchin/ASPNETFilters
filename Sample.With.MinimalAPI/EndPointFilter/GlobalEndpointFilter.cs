namespace Sample.With.MinimalAPI.EndPointFilter;

public class GlobalEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<GlobalEndpointFilter>();

        logger.LogInformation($"{nameof(GlobalEndpointFilter)} Invoked");

        return await next(context);
    }
}