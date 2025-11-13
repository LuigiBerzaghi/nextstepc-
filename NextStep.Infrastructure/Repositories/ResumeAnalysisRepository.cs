using Microsoft.EntityFrameworkCore;
using NextStep.Application.Interfaces.Repositories;
using NextStep.Domain.Entities;
using NextStep.Infrastructure.Persistence;

namespace NextStep.Infrastructure.Repositories;

public class ResumeAnalysisRepository : IResumeAnalysisRepository
{
    private readonly NextStepDbContext _context;

    public ResumeAnalysisRepository(NextStepDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(ResumeAnalysis analysis, CancellationToken cancellationToken) =>
        _context.ResumeAnalyses.AddAsync(analysis, cancellationToken).AsTask();

    public Task<ResumeAnalysis?> GetLatestAsync(int userId, CancellationToken cancellationToken) =>
        _context.ResumeAnalyses
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.AnalyzedAt)
            .FirstOrDefaultAsync(cancellationToken);
}
