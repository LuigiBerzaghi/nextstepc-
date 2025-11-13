using NextStep.Domain.Enums;

namespace NextStep.Domain.Entities;

public class JourneyStep
{
    public int Id { get; set; }
    public int JourneyId { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Objective { get; set; } = string.Empty;
    public string Resources { get; set; } = string.Empty;
    public string EstimatedTime { get; set; } = string.Empty;
    public int Progress { get; private set; }
    public JourneyStepStatus Status { get; private set; } = JourneyStepStatus.Pending;
    public DateTime? UpdatedAt { get; set; }

    public void UpdateProgress(int progress)
    {
        Progress = Math.Clamp(progress, 0, 100);

        Status = Progress switch
        {
            >= 100 => JourneyStepStatus.Completed,
            > 0 => JourneyStepStatus.InProgress,
            _ => JourneyStepStatus.Pending
        };

        UpdatedAt = DateTime.UtcNow;
    }
}
