namespace Restaurant.API.Dto.Responses;

public sealed class LoginEmployeeResponse
{
    public Guid EmployeeId { get; set; }
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public required string UserEmail { get; set; }
    public bool IsVerified { get; set; }
    public required string EmployeeRole { get; set; }
    public required string AccessToken { get; set; }
}
