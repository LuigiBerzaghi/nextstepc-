namespace NextStep.Domain.Entities;

public class ResumeAnalysis
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string SummaryJson { get; set; } = string.Empty;
    public DateTime AnalyzedAt { get; set; } = DateTime.UtcNow;
}
