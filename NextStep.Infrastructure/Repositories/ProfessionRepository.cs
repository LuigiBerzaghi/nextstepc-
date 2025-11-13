using Microsoft.EntityFrameworkCore;
using NextStep.Application.Interfaces.Repositories;
using NextStep.Domain.Entities;
using NextStep.Infrastructure.Persistence;

namespace NextStep.Infrastructure.Repositories;

public class ProfessionRepository : IProfessionRepository
{
    private readonly NextStepDbContext _context;

    public ProfessionRepository(NextStepDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyCollection<Profession> Professions, int TotalCount)> SearchAsync(string? term, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Professions.Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(term))
        {
            var like = $"%{term}%";
            query = query.Where(p => EF.Functions.Like(p.Title, like) || EF.Functions.Like(p.Category, like));
        }

        var total = await query.CountAsync(cancellationToken);
        var professions = await query
            .OrderBy(p => p.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (professions, total);
    }
}
