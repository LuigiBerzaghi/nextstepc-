namespace NextStep.Application.DTOs.Auth;

public class UserSummaryDto
{
    public int Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? Name { get; init; }
    public string? CurrentJob { get; init; }
}
