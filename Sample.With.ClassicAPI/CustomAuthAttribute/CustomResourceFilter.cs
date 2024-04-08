using Microsoft.AspNetCore.Mvc.Filters;

namespace Sample.With.ClassicAPI.CustomAuthAttribute;

public class CustomResourceFilter : Attribute, IResourceFilter
{

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<CustomResourceFilter>();
        logger.LogInformation($"{nameof(OnResourceExecuted)} Invoked");
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<CustomResourceFilter>();
        logger.LogInformation($"{nameof(OnResourceExecuting)} Invoked");
    }
}
