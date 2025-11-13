using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextStep.Application.DTOs.Auth;
using NextStep.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace NextStep.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/auth")]
[SwaggerTag("Fluxo de autenticação simplificada da NextStep")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Realiza login simplificado",
        Description = "Se o usuário não existir ele é criado automaticamente e um JWT é retornado."
    )]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login solicitado para {Email}", request.Email);
        var response = await _authService.LoginAsync(request, cancellationToken);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [SwaggerOperation(
        Summary = "Registra um novo usuário",
        Description = "Cria ou reativa um usuário e retorna os dados básicos junto do token JWT."
    )]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registro solicitado para {Email}", request.Email);
        var response = await _authService.RegisterAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Login), new { version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0" }, response);
    }
}
