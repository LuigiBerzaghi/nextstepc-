using NextStep.Application.Interfaces.Repositories;
using NextStep.Infrastructure.Persistence;

namespace NextStep.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly NextStepDbContext _context;

    public UnitOfWork(NextStepDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
