using System.Text.Json;
using NextStep.Application.DTOs.Resume;
using NextStep.Application.Interfaces.Repositories;
using NextStep.Application.Interfaces.Services;
using NextStep.Domain.Entities;

namespace NextStep.Application.Services;

public class ResumeService : IResumeService
{
    private readonly IResumeAnalysisRepository _resumeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ResumeService(IResumeAnalysisRepository resumeRepository, IUnitOfWork unitOfWork)
    {
        _resumeRepository = resumeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResumeAnalysisDto> AnalyzeAsync(int userId, ResumeUploadRequest request, CancellationToken cancellationToken)
    {
        var payload = new
        {
            skills = ExtractHighlights(request.ResumeText),
            gaps = SuggestGaps(request.ResumeText),
            careers = SuggestCareers(request.ResumeText)
        };

        var analysis = new ResumeAnalysis
        {
            UserId = userId,
            SummaryJson = JsonSerializer.Serialize(payload),
            AnalyzedAt = DateTime.UtcNow
        };

        await _resumeRepository.AddAsync(analysis, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(analysis);
    }

    public async Task<ResumeAnalysisDto?> GetLatestAsync(int userId, CancellationToken cancellationToken)
    {
        var analysis = await _resumeRepository.GetLatestAsync(userId, cancellationToken);
        return analysis is null ? null : Map(analysis);
    }

    private static ResumeAnalysisDto Map(ResumeAnalysis analysis) =>
        new()
        {
            Id = analysis.Id,
            UserId = analysis.UserId,
            Summary = JsonSerializer.Deserialize<JsonElement>(analysis.SummaryJson),
            AnalyzedAt = analysis.AnalyzedAt
        };

    private static IEnumerable<string> ExtractHighlights(string text)
    {
        var keywords = new[] { "cloud", "ai", "data", "python", "c#", "leadership", "design" };
        return keywords.Where(k => text.Contains(k, StringComparison.OrdinalIgnoreCase));
    }

    private static IEnumerable<string> SuggestGaps(string text)
    {
        var suggestions = new List<string> { "Soft skills de comunicação", "Gestão de tempo" };
        if (!text.Contains("cloud", StringComparison.OrdinalIgnoreCase))
        {
            suggestions.Add("Arquitetura Cloud");
        }

        if (!text.Contains("ai", StringComparison.OrdinalIgnoreCase))
        {
            suggestions.Add("Fundamentos de IA Generativa");
        }

        return suggestions;
    }

    private static IEnumerable<string> SuggestCareers(string text)
    {
        if (text.Contains("data", StringComparison.OrdinalIgnoreCase))
        {
            return new[] { "Data Product Manager", "Analytics Lead" };
        }

        return new[] { "AI Product Strategist", "Innovation Specialist" };
    }
}
