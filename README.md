# ASP.NET Filter

This project illustrates the implementation various types of filter in C#.NET, showcasing two ASP.NET API approaches:
- ASP.NET MVC
- Minimal API.

As the transition from ASP.NET MVC to Minimal APIs continues in recent years, it's important to note that some filters may not be supported in the Minimal API framework.

Currently, in Microsoft documentation, these filters are often grouped together, but it's conceivable that additional filters from the MVC pipeline may become available for use in Minimal APIs in the future.

## Available filters type for ASP.NET MVC

Here a list of all the filter type, ordered by execution order:

 1. **Authorization filters:** used to verify that a user is authorized to access a particular controller.
 2.  **Resource filters:** Run after Auth filter. Execution wraps most of the filter pipeline.
 3.  **Action filters:** Their execution surrounds the execution of action methods.are useful for implementing cross-cutting concerns such as logging.
 4. **Exception filters:** Exception filters are used to handle exceptions that occur during the execution of an action method. They run only if an unhandled exception is thrown.
 5. **Result filters:** Result filters run before and after the execution of the action method, but they specifically deal with the result returned by the action.

## Available filters type for Minimal API

 1. **Authorization filters:** used to verify that a user is authorized to access a particular controller.
 2. **Endpoint filters**: used to chance arguments or result from passed into or from the action.

# ASP.NET MVC Filters

## Authorization

Authorization Filters in .NET serve to verify whether a user is authorized to access a specific controller. 
They are the initial filter triggered in the pipeline and are commonly employed when implementing token-based authorization flows.


Below is the creation of a custom auth attribute:

```csharp
public class CustomAuthorizationFilter : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<CustomAuthorizationFilter>();

        logger.LogInformation($"{nameof(OnAuthorizationAsync)} Invoked");
    }
}

```
Once defined, you can apply it atop your controllers:

```csharp
    [CustomAuthorizationFilter]
```

## Resource

Resource filters execute their logic immediately after the execution of an authentication filter and encapsulate the majority of other filters. As they operate just before the model binding step, a useful application of these filters is to influence model binding.


Below is the creation of a custom auth attribute:

```csharp
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
```
Once defined, you can apply it atop your controllers:

```csharp
    [CustomResourceFilter]
```

## Action

Action filters are used to perform some specific tasks before or after the action method is executed. These filters can be added to different scope levels: Global, Action, Controller.

Below is the creation of a custom auth attribute:

```csharp
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
```

Once defined, you can apply it atop your controllers:

```csharp
  [CustomActionFilter]
```

## Exception

Exception filters are employed to manage exceptions arising during request processing. They can be assigned to a controller or an action method through an attribute.

Below is the creation of a custom auth attribute:

```csharp
public class CustomExceptionFilter : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<CustomExceptionFilter>();
        logger.LogInformation($"{nameof(OnException)} Invoked");
    }
}

```
Once defined, you can apply it atop your controllers:

```csharp
    [CustomExceptionFilter]
```

## Result

Result Filters are a specialized type of filter that operates after the action method has been executed, but prior to processing and transmitting the result to the client. Essentially, they function both before and after executing action results.

Below is the creation of a custom auth attribute:

```csharp
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

```
Once defined, you can apply it atop your controllers:

```csharp
 [CustomResultFilter]
```



# Minimal API

## Authorization

Authorization Filters in .NET serve to verify whether a user is authorized to access a specific controller. 
They are the initial filter triggered in the pipeline and are commonly employed when implementing token-based authorization flows.

To craft a custom auth attribute with Minimal API, certain prerequisites must be established:

```csharp
public class CustomAuthRequirementInput : IAuthorizationRequirement
{
    public CustomAuthRequirementInput(string condition) => Condition = condition;
    public string Condition { get; set; }
}
```

Subsequently, a custom AuthorizationHandler needs implementation with the aforementioned input:

```csharp
public class CustomAuthorizationHandler(ICustomAuthService customAuthService) : AuthorizationHandler<CustomAuthRequirementInput>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthRequirementInput requirement)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<CustomAuthorizationHandler>();

        logger.LogInformation($"{nameof(CustomAuthorizationHandler)} Invoked");

        context.Succeed(requirement);
    }

}
```
Finally, adjust your program.cs as follows:

```csharp
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("Test", p => p.AddRequirements(new CustomAuthRequirementInput("Test")));
});

// .......

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.RequireAuthorization("Test")
.WithOpenApi();
```

## Endpoint

Endpoint filters run at the same level as action filters and are exclusively available with the minimal API. They encapsulate a group of endpoints. From a classic ASP.NET MVC perspective, they function similarly to a filter that acts as a controller filter.


Below is the creation of a custom auth attribute:

```csharp
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
```

Global Registration:


```csharp
var global = app
    .MapGroup(string.Empty)
    .AddEndpointFilter<EndpointFilter>();
```

Endpoint Registration:

```csharp
global.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.AddEndpointFilter<EndpointFilter>()
.WithOpenApi();
```


