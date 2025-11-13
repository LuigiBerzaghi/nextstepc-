using System.Net;
using NextStep.Application.DTOs.Profile;
using NextStep.Application.Exceptions;
using NextStep.Application.Interfaces.Repositories;
using NextStep.Application.Interfaces.Services;
using NextStep.Application.Security;

namespace NextStep.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProfileResponse> GetProfileAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                   ?? throw new AppException("Usuário não encontrado.", HttpStatusCode.NotFound);

        return Map(user);
    }

    public async Task<ProfileResponse> UpdateProfileAsync(int userId, UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                   ?? throw new AppException("Usuário não encontrado.", HttpStatusCode.NotFound);

        if (!string.IsNullOrWhiteSpace(request.NewEmail) && !request.NewEmail.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _userRepository.GetByEmailAsync(request.NewEmail, cancellationToken);
            if (existing is not null)
            {
                throw new AppException("Já existe um usuário com esse e-mail.", HttpStatusCode.UnprocessableEntity);
            }
            user.SetEmail(request.NewEmail);
        }

        if (!string.IsNullOrEmpty(request.NewPassword))
        {
            if (string.IsNullOrEmpty(request.CurrentPassword) ||
                !PasswordHasher.Verify(request.CurrentPassword, user.PasswordHash))
            {
                throw new AppException("Senha atual inválida.", HttpStatusCode.Unauthorized);
            }

            user.SetPassword(PasswordHasher.Hash(request.NewPassword));
        }

        user.Name = request.Name ?? user.Name;
        user.CurrentJob = request.CurrentJob ?? user.CurrentJob;
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(user);
    }

    public async Task DeleteProfileAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                   ?? throw new AppException("Usuário não encontrado.", HttpStatusCode.NotFound);

        _userRepository.Remove(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static ProfileResponse Map(Domain.Entities.User user) =>
        new()
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            CurrentJob = user.CurrentJob,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
}
