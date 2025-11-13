using NextStep.Application.DTOs.Journeys;

namespace NextStep.Application.DTOs.Dashboard;

public class DashboardResponse
{
    public string Email { get; init; } = string.Empty;
    public string? Name { get; init; }
    public string? CurrentJob { get; init; }
    public int TotalJourneys { get; init; }
    public int CompletedJourneys { get; init; }
    public double AverageProgress { get; init; }
    public JourneyStepDto? NextStep { get; init; }
}
