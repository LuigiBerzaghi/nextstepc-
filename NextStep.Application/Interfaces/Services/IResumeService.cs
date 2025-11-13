using NextStep.Application.DTOs.Resume;

namespace NextStep.Application.Interfaces.Services;

public interface IResumeService
{
    Task<ResumeAnalysisDto> AnalyzeAsync(int userId, ResumeUploadRequest request, CancellationToken cancellationToken);
    Task<ResumeAnalysisDto?> GetLatestAsync(int userId, CancellationToken cancellationToken);
}
