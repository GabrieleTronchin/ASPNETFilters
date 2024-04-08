using Microsoft.AspNetCore.Authorization;

namespace Sample.With.MinimalAPI.CustomAuthFilter;

public class CustomAuthRequirementInput : IAuthorizationRequirement
{
    public CustomAuthRequirementInput(string condition) => Condition = condition;
    public string Condition { get; set; }
}