using NextStep.Application.Common;
using NextStep.Application.DTOs.Journeys;

namespace NextStep.Application.Interfaces.Services;

public interface IJourneyService
{
    Task<JourneyDto> CreateJourneyAsync(int userId, CreateJourneyRequest request, CancellationToken cancellationToken);
    Task<JourneyDto?> GetActiveJourneyAsync(int userId, CancellationToken cancellationToken);
    Task<JourneyDto?> UpdateStepProgressAsync(int userId, int stepId, UpdateJourneyStepProgressRequest request, CancellationToken cancellationToken);
    Task<PagedResult<JourneyDto>> GetJourneyHistoryAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken);
}
