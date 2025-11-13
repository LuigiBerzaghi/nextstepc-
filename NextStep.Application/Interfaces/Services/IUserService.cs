using NextStep.Application.DTOs.Profile;

namespace NextStep.Application.Interfaces.Services;

public interface IUserService
{
    Task<ProfileResponse> GetProfileAsync(int userId, CancellationToken cancellationToken);
    Task<ProfileResponse> UpdateProfileAsync(int userId, UpdateProfileRequest request, CancellationToken cancellationToken);
    Task DeleteProfileAsync(int userId, CancellationToken cancellationToken);
}
