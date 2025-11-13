using NextStep.Domain.Enums;

namespace NextStep.Domain.Entities;

public class Journey
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string DesiredJob { get; set; } = string.Empty;
    public JourneyStatus Status { get; private set; } = JourneyStatus.Active;
    public int OverallProgress { get; private set; }
    public int TotalSteps { get; private set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<JourneyStep> Steps { get; set; } = new List<JourneyStep>();

    public JourneyStep? GetNextStep() =>
        Steps
            .Where(s => s.Status is JourneyStepStatus.Pending or JourneyStepStatus.InProgress)
            .OrderBy(s => s.Order)
            .FirstOrDefault();

    public void SetTotalSteps()
    {
        TotalSteps = Steps.Count;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecalculateProgress()
    {
        if (Steps.Count == 0)
        {
            OverallProgress = 0;
            UpdatedAt = DateTime.UtcNow;
            return;
        }

        OverallProgress = (int)Math.Round(Steps.Average(s => s.Progress));
        TotalSteps = Steps.Count;
        UpdatedAt = DateTime.UtcNow;

        if (OverallProgress >= 100 && Steps.All(s => s.Status == JourneyStepStatus.Completed))
        {
            Status = JourneyStatus.Completed;
        }
    }

    public void Archive()
    {
        Status = JourneyStatus.Archived;
        UpdatedAt = DateTime.UtcNow;
    }
}
