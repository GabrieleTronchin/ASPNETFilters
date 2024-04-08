using Microsoft.AspNetCore.Authorization;

namespace Sample.With.MinimalAPI.CustomAuthFilter;

public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthRequirementInput>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthRequirementInput requirement)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<CustomAuthorizationHandler>();

        logger.LogInformation($"{nameof(CustomAuthorizationHandler)} Invoked");

        context.Succeed(requirement);
    }

}