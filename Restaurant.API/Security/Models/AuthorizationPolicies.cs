namespace Restaurant.API.Security.Models;

public sealed class AuthorizationPolicies
{
    public const string RequireCustomer = "RequireCustomer";
    public const string RequireEmployee = "RequireEmployee";
    public const string RequireEmployeeManager = "RequireEmployeeManager";
    public const string RequireEmployeeWaiter = "RequireEmployeeWaiter";

    private AuthorizationPolicies() { }
}