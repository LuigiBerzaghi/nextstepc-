using NextStep.Domain.Enums;

namespace NextStep.Application.DTOs.Journeys;

public class JourneyStepDto
{
    public int Id { get; init; }
    public int Order { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Objective { get; init; } = string.Empty;
    public string Resources { get; init; } = string.Empty;
    public string EstimatedTime { get; init; } = string.Empty;
    public int Progress { get; init; }
    public JourneyStepStatus Status { get; init; }
}
