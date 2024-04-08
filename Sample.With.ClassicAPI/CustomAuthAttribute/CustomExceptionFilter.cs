using Microsoft.AspNetCore.Mvc.Filters;

namespace Sample.With.ClassicAPI.CustomAuthAttribute;

public class CustomExceptionFilter : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<CustomExceptionFilter>();
        logger.LogInformation($"{nameof(OnException)} Invoked");
    }
}
