using Microsoft.AspNetCore.Mvc.Filters;

namespace Sample.With.ClassicAPI.CustomAuthAttribute;

public class CustomAsyncActionFilter : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<CustomAsyncActionFilter>();

        logger.LogInformation($"{nameof(OnActionExecutionAsync)} Executed Before Invocation");
        await next();
        logger.LogInformation($"{nameof(OnActionExecutionAsync)} Executed After Invocation");
    }
}
