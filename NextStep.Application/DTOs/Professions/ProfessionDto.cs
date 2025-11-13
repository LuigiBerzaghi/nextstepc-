namespace NextStep.Application.DTOs.Professions;

public class ProfessionDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
