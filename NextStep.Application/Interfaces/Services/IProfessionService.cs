using NextStep.Application.Common;
using NextStep.Application.DTOs.Professions;

namespace NextStep.Application.Interfaces.Services;

public interface IProfessionService
{
    Task<PagedResult<ProfessionDto>> SearchAsync(string? term, int pageNumber, int pageSize, CancellationToken cancellationToken);
}
