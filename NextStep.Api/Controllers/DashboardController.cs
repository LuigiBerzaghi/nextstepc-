using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextStep.Application.DTOs.Dashboard;
using NextStep.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace NextStep.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/dashboard")]
[SwaggerTag("Resumo consolidado do aluno NextStep")]
public class DashboardController : ApiControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(DashboardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Retorna o painel do usuário",
        Description = "Consolida stats de jornadas, próximo passo e dados básicos do perfil."
    )]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var response = await _dashboardService.GetDashboardAsync(GetUserId(), cancellationToken);
        return Ok(response);
    }
}
