using Microsoft.AspNetCore.Mvc.Filters;

namespace Sample.With.ClassicAPI.CustomAuthAttribute;

public class CustomResultFilter : Attribute, IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<CustomResultFilter>();

        logger.LogInformation($"{nameof(OnResultExecuting)} Invoked");
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<CustomResultFilter>();

        logger.LogInformation($"{nameof(OnResultExecuted)} Invoked");
    }
}
