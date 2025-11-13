using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextStep.Application.DTOs.Resume;
using NextStep.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace NextStep.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/resume")]
[SwaggerTag("Upload e consulta das análises de currículo")]
public class ResumeController : ApiControllerBase
{
    private readonly IResumeService _resumeService;

    public ResumeController(IResumeService resumeService)
    {
        _resumeService = resumeService;
    }

    [HttpPost("upload")]
    [ProducesResponseType(typeof(ResumeAnalysisDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Envia o currículo para análise",
        Description = "Gera uma análise fake com skills, gaps e carreiras e persiste no histórico do usuário."
    )]
    public async Task<IActionResult> Upload([FromBody] ResumeUploadRequest request, CancellationToken cancellationToken)
    {
        var analysis = await _resumeService.AnalyzeAsync(GetUserId(), request, cancellationToken);
        return CreatedAtAction(nameof(GetLatestAnalysis), new { version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0" }, analysis);
    }

    [HttpGet("analysis/latest")]
    [ProducesResponseType(typeof(ResumeAnalysisDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Obtém a última análise de currículo",
        Description = "Retorna o JSON consolidado com os insights mais recentes do usuário autenticado."
    )]
    public async Task<IActionResult> GetLatestAnalysis(CancellationToken cancellationToken)
    {
        var analysis = await _resumeService.GetLatestAsync(GetUserId(), cancellationToken);
        return analysis is null
            ? NotFound(new { error = "NOT_FOUND", message = "Nenhuma análise encontrada." })
            : Ok(analysis);
    }
}
