using NextStep.Domain.Entities;

namespace NextStep.Application.Interfaces.Repositories;

public interface IResumeAnalysisRepository
{
    Task AddAsync(ResumeAnalysis analysis, CancellationToken cancellationToken);
    Task<ResumeAnalysis?> GetLatestAsync(int userId, CancellationToken cancellationToken);
}
