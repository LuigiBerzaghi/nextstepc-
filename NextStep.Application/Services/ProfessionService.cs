using NextStep.Application.Common;
using NextStep.Application.DTOs.Professions;
using NextStep.Application.Interfaces.Repositories;
using NextStep.Application.Interfaces.Services;

namespace NextStep.Application.Services;

public class ProfessionService : IProfessionService
{
    private readonly IProfessionRepository _professionRepository;

    public ProfessionService(IProfessionRepository professionRepository)
    {
        _professionRepository = professionRepository;
    }

    public async Task<PagedResult<ProfessionDto>> SearchAsync(string? term, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var (professions, total) = await _professionRepository.SearchAsync(term, pageNumber, pageSize, cancellationToken);
        var data = professions
            .Select(p => new ProfessionDto
            {
                Id = p.Id,
                Title = p.Title,
                Category = p.Category,
                Description = p.Description
            })
            .ToList();

        return new PagedResult<ProfessionDto>
        {
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = total
        };
    }
}
