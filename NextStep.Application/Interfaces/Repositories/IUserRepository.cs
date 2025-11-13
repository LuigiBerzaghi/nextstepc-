using NextStep.Domain.Entities;

namespace NextStep.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<User?> GetWithJourneysAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    void Update(User user);
    void Remove(User user);
}
