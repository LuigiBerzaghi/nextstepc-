using System.Net;
using NextStep.Application.DTOs.Auth;
using NextStep.Application.Exceptions;
using NextStep.Application.Interfaces.Repositories;
using NextStep.Application.Interfaces.Services;
using NextStep.Application.Security;
using NextStep.Domain.Entities;

namespace NextStep.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken)
                   ?? throw new AppException("Credenciais inválidas.", HttpStatusCode.Unauthorized);

        if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new AppException("Credenciais inválidas.", HttpStatusCode.Unauthorized);
        }

        return BuildAuthResponse(user);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existing is not null)
        {
            throw new AppException("Usuário já cadastrado.", HttpStatusCode.UnprocessableEntity);
        }

        var user = new User
        {
            Name = request.Name,
            CurrentJob = request.CurrentJob,
            CreatedAt = DateTime.UtcNow
        };
        user.SetEmail(request.Email);
        user.SetPassword(PasswordHasher.Hash(request.Password));

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BuildAuthResponse(user);
    }

    private AuthResponse BuildAuthResponse(User user) =>
        new()
        {
            Token = _tokenService.GenerateToken(user),
            User = new UserSummaryDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                CurrentJob = user.CurrentJob
            }
        };
}
