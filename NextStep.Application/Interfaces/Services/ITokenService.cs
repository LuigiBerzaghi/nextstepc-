using NextStep.Domain.Entities;

namespace NextStep.Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
