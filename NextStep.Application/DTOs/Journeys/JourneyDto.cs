using NextStep.Domain.Enums;

namespace NextStep.Application.DTOs.Journeys;

public class JourneyDto
{
    public int Id { get; init; }
    public string DesiredJob { get; init; } = string.Empty;
    public JourneyStatus Status { get; init; }
    public int OverallProgress { get; init; }
    public int TotalSteps { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public JourneyStepDto? NextStep { get; init; }
    public IReadOnlyCollection<JourneyStepDto> Steps { get; init; } = Array.Empty<JourneyStepDto>();
}
