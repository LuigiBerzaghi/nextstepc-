using NextStep.Domain.Entities;

namespace NextStep.Application.Interfaces.Repositories;

public interface IProfessionRepository
{
    Task<(IReadOnlyCollection<Profession> Professions, int TotalCount)> SearchAsync(
        string? term,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}
