using NextStep.Domain.Entities;

namespace NextStep.Application.Interfaces.Repositories;

public interface IJourneyRepository
{
    Task<Journey?> GetActiveJourneyAsync(int userId, CancellationToken cancellationToken);
    Task<Journey?> GetByIdAsync(int journeyId, CancellationToken cancellationToken);
    Task AddAsync(Journey journey, CancellationToken cancellationToken);
    void Update(Journey journey);
    Task<JourneyStep?> GetStepByIdAsync(int stepId, CancellationToken cancellationToken);
    Task<(IReadOnlyCollection<Journey> Journeys, int TotalCount)> GetHistoryAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken);
}
