using NextStep.Application.DTOs.Auth;

namespace NextStep.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
}
