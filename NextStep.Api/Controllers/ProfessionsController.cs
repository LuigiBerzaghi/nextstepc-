using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextStep.Api.Infrastructure;
using NextStep.Api.Models;
using NextStep.Application.DTOs.Professions;
using NextStep.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace NextStep.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/professions")]
[SwaggerTag("Catálogo de profissões sugeridas para o Futuro do Trabalho")]
public class ProfessionsController : ApiControllerBase
{
    private const string ControllerName = "Professions";
    private readonly IProfessionService _professionService;
    private readonly ILinkBuilder _linkBuilder;

    public ProfessionsController(IProfessionService professionService, ILinkBuilder linkBuilder)
    {
        _professionService = professionService;
        _linkBuilder = linkBuilder;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<ProfessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Pesquisa profissões sugeridas",
        Description = "Permite filtrar por título e categoria, retornando paginação e links HATEOAS."
    )]
    public async Task<IActionResult> Get([FromQuery] string? search, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _professionService.SearchAsync(search, Math.Max(1, pageNumber), Math.Clamp(pageSize, 1, 50), cancellationToken);

        var response = new PagedResponse<ProfessionDto>
        {
            Data = result.Data,
            Pagination = new PaginationMetadata
            {
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages
            },
            Links = _linkBuilder.BuildPaginationLinks(HttpContext, nameof(Get), ControllerName, result.PageNumber, result.PageSize, result.TotalPages, new { search })
        };

        return Ok(response);
    }
}
