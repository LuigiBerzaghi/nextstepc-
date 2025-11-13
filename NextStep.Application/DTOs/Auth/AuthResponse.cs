namespace NextStep.Application.DTOs.Auth;

public class AuthResponse
{
    public string Token { get; init; } = string.Empty;
    public UserSummaryDto User { get; init; } = new();
}
