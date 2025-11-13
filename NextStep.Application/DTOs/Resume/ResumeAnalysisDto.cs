using System.Text.Json;

namespace NextStep.Application.DTOs.Resume;

public class ResumeAnalysisDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public JsonElement Summary { get; init; }
    public DateTime AnalyzedAt { get; init; }
}
