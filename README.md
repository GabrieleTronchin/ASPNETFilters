# ASP.NET Filter

This document illustrates the implementation various types of filter in C#.NET, showcasing two ASP.NET API approaches:
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



## Minimal API

To craft a custom auth attribute with Minimal API, certain prerequisites must be established:

```csharp
public class MyCustomAuthRequirementInput : IAuthorizationRequirement
{
    public MyCustomAuthRequirementInput(string condition) => Condition = condition;
    public string Condition { get; set; }
}
```

Subsequently, a custom AuthorizationHandler needs implementation with the aforementioned input:

```csharp
public class MyCustomAuthorizationHandler(ICustomAuthService customAuthService) : AuthorizationHandler<MyCustomAuthRequirementInput>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MyCustomAuthRequirementInput requirement)
    {

        if (await customAuthService.CheckIfAllowed(requirement.Condition))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

    }

}
```
Finally, adjust your program.cs as follows:

```csharp
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("Test", p => p.AddRequirements(new MyCustomAuthRequirementInput("Test")));
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
```csharp
