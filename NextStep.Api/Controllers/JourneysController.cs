using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextStep.Api.Infrastructure;
using NextStep.Api.Models;
using NextStep.Application.Common;
using NextStep.Application.DTOs.Journeys;
using NextStep.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace NextStep.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/journeys")]
[SwaggerTag("Criação e acompanhamento das jornadas personalizadas")]
public class JourneysController : ApiControllerBase
{
    private const string ControllerName = "Journeys";

    private readonly IJourneyService _journeyService;
    private readonly ILinkBuilder _linkBuilder;
    private readonly LinkGenerator _linkGenerator;

    public JourneysController(IJourneyService journeyService, ILinkBuilder linkBuilder, LinkGenerator linkGenerator)
    {
        _journeyService = journeyService;
        _linkBuilder = linkBuilder;
        _linkGenerator = linkGenerator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Cria uma nova jornada personalizada",
        Description = "Arquiva a jornada ativa, gera novos passos inteligentes e retorna os links principais."
    )]
    public async Task<IActionResult> Create([FromBody] CreateJourneyRequest request, CancellationToken cancellationToken)
    {
        var journey = await _journeyService.CreateJourneyAsync(GetUserId(), request, cancellationToken);
        var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0";

        var body = new
        {
            data = journey,
            links = new Dictionary<string, string?>
            {
                ["self"] = _linkGenerator.GetUriByAction(HttpContext, nameof(GetActive), ControllerName, new { version }),
                ["history"] = _linkGenerator.GetUriByAction(HttpContext, nameof(GetHistory), ControllerName, new { version, pageNumber = 1, pageSize = 10 })
            }
        };

        return CreatedAtAction(nameof(GetActive), new { version }, body);
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Busca a jornada ativa",
        Description = "Retorna progresso geral, próximo passo e links HATEOAS para atualização e histórico."
    )]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var journey = await _journeyService.GetActiveJourneyAsync(GetUserId(), cancellationToken);
        if (journey is null)
        {
            return NotFound(new { error = "NOT_FOUND", message = "Nenhuma jornada ativa." });
        }

        var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0";
        var links = new Dictionary<string, string?>
        {
            ["self"] = _linkGenerator.GetUriByAction(HttpContext, nameof(GetActive), ControllerName, new { version }),
            ["updateStep"] = journey.NextStep is null
                ? null
                : _linkGenerator.GetUriByAction(HttpContext, nameof(UpdateStepProgress), ControllerName, new { version, stepId = journey.NextStep.Id }),
            ["history"] = _linkGenerator.GetUriByAction(HttpContext, nameof(GetHistory), ControllerName, new { version, pageNumber = 1, pageSize = 10 })
        };

        return Ok(new { data = journey, links });
    }

    [HttpPatch("steps/{stepId:int}/progress")]
    [ProducesResponseType(typeof(JourneyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Atualiza o progresso de um passo",
        Description = "Recalcula automaticamente o progresso geral da jornada após salvar o passo."
    )]
    public async Task<IActionResult> UpdateStepProgress(int stepId, [FromBody] UpdateJourneyStepProgressRequest request, CancellationToken cancellationToken)
    {
        var journey = await _journeyService.UpdateStepProgressAsync(GetUserId(), stepId, request, cancellationToken);
        return Ok(journey);
    }

    [HttpGet("history")]
    [ProducesResponseType(typeof(PagedResponse<JourneyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Lista o histórico de jornadas",
        Description = "Retorna jornadas concluídas/arquivadas com metadados de paginação e links HATEOAS."
    )]
    public async Task<IActionResult> GetHistory([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var result = await _journeyService.GetJourneyHistoryAsync(GetUserId(), pageNumber, pageSize, cancellationToken);

        var response = new PagedResponse<JourneyDto>
        {
            Data = result.Data,
            Pagination = new PaginationMetadata
            {
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages
            },
            Links = _linkBuilder.BuildPaginationLinks(HttpContext, nameof(GetHistory), ControllerName, result.PageNumber, result.PageSize, result.TotalPages)
        };

        return Ok(response);
    }
}
