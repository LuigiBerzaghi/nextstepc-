namespace NextStep.Application.DTOs.Profile;

public class ProfileResponse
{
    public int Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? Name { get; init; }
    public string? CurrentJob { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
