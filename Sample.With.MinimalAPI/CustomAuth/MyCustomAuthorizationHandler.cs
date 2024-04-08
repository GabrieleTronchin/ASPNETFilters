using Microsoft.AspNetCore.Authorization;

namespace Sample.With.MinimalAPI.CustomAuth;

public class MyCustomAuthorizationHandler : AuthorizationHandler<MyCustomAuthRequirementInput>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MyCustomAuthRequirementInput requirement)
    {
        context.Succeed(requirement);
    }

}