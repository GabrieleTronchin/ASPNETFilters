using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;


namespace Sample.With.ClassicAPI.CustomAuthAttribute;

public class CustomAuthorizationFilter : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<CustomAuthorizationFilter>();

        logger.LogInformation($"{nameof(OnAuthorizationAsync)} Invoked");
    }
}
