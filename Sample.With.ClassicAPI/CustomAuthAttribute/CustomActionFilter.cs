using Microsoft.AspNetCore.Mvc.Filters;

namespace Sample.With.ClassicAPI.CustomAuthAttribute;

public class CustomActionFilter : Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<CustomActionFilter>();

        logger.LogInformation($"{nameof(OnActionExecuting)} Invoked");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<CustomActionFilter>();

        logger.LogInformation($"{nameof(OnActionExecuted)} Invoked");
    }
}
