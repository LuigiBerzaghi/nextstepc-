using Microsoft.EntityFrameworkCore;
using NextStep.Application.Interfaces.Repositories;
using NextStep.Domain.Entities;
using NextStep.Domain.Enums;
using NextStep.Infrastructure.Persistence;

namespace NextStep.Infrastructure.Repositories;

public class JourneyRepository : IJourneyRepository
{
    private readonly NextStepDbContext _context;

    public JourneyRepository(NextStepDbContext context)
    {
        _context = context;
    }

    public Task<Journey?> GetActiveJourneyAsync(int userId, CancellationToken cancellationToken) =>
        _context.Journeys
            .Include(j => j.Steps)
            .FirstOrDefaultAsync(j => j.UserId == userId && j.Status == JourneyStatus.Active, cancellationToken);

    public Task<Journey?> GetByIdAsync(int journeyId, CancellationToken cancellationToken) =>
        _context.Journeys
            .Include(j => j.Steps)
            .FirstOrDefaultAsync(j => j.Id == journeyId, cancellationToken);

    public Task AddAsync(Journey journey, CancellationToken cancellationToken) =>
        _context.Journeys.AddAsync(journey, cancellationToken).AsTask();

    public void Update(Journey journey) => _context.Journeys.Update(journey);

    public Task<JourneyStep?> GetStepByIdAsync(int stepId, CancellationToken cancellationToken) =>
        _context.JourneySteps.FirstOrDefaultAsync(s => s.Id == stepId, cancellationToken);

    public async Task<(IReadOnlyCollection<Journey> Journeys, int TotalCount)> GetHistoryAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Journeys
            .Include(j => j.Steps)
            .Where(j => j.UserId == userId && j.Status != JourneyStatus.Active)
            .OrderByDescending(j => j.CreatedAt);

        var total = await query.CountAsync(cancellationToken);
        var journeys = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (journeys, total);
    }
}
