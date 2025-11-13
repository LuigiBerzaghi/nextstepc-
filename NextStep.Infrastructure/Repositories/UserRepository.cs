using Microsoft.EntityFrameworkCore;
using NextStep.Application.Interfaces.Repositories;
using NextStep.Domain.Entities;
using NextStep.Infrastructure.Persistence;

namespace NextStep.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NextStepDbContext _context;

    public UserRepository(NextStepDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public Task<User?> GetWithJourneysAsync(int id, CancellationToken cancellationToken) =>
        _context.Users
            .Include(u => u.Journeys)
                .ThenInclude(j => j.Steps)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public Task AddAsync(User user, CancellationToken cancellationToken) =>
        _context.Users.AddAsync(user, cancellationToken).AsTask();

    public void Update(User user) => _context.Users.Update(user);

    public void Remove(User user) => _context.Users.Remove(user);
}
