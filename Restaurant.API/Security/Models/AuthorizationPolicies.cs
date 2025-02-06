namespace Restaurant.API.Security.Models;

public sealed class AuthorizationPolicies
{
    public const string RequireCustomer = "RequireCustomer";
    public const string RequireAdmin = "RequireAdmin";

    private AuthorizationPolicies() { }
}