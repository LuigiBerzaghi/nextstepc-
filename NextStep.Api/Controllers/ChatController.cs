using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextStep.Api.Infrastructure;
using NextStep.Api.Models;
using NextStep.Application.DTOs.Chat;
using NextStep.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace NextStep.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/chat")]
[SwaggerTag("Mentor AI simulado e histórico de conversas")]
public class ChatController : ApiControllerBase
{
    private const string ControllerName = "Chat";

    private readonly IChatService _chatService;
    private readonly ILinkBuilder _linkBuilder;

    public ChatController(IChatService chatService, ILinkBuilder linkBuilder)
    {
        _chatService = chatService;
        _linkBuilder = linkBuilder;
    }

    [HttpPost("send")]
    [ProducesResponseType(typeof(ChatResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Envia mensagem ao Mentor AI",
        Description = "Armazena a interação e retorna a resposta fake gerada pelo backend."
    )]
    public async Task<IActionResult> Send([FromBody] ChatSendRequest request, CancellationToken cancellationToken)
    {
        var response = await _chatService.SendMessageAsync(GetUserId(), request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("history")]
    [ProducesResponseType(typeof(PagedResponse<ChatMessageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Lista o histórico da conversa",
        Description = "Retorna mensagens paginadas por conversationId com links de navegação."
    )]
    public async Task<IActionResult> History([FromQuery][Required] string conversationId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _chatService.GetHistoryAsync(GetUserId(), conversationId, Math.Max(1, pageNumber), Math.Clamp(pageSize, 1, 50), cancellationToken);

        var links = _linkBuilder.BuildPaginationLinks(
            HttpContext,
            nameof(History),
            ControllerName,
            result.PageNumber,
            result.PageSize,
            result.TotalPages,
            new { conversationId });

        var response = new PagedResponse<ChatMessageDto>
        {
            Data = result.Data,
            Pagination = new PaginationMetadata
            {
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages
            },
            Links = links
        };

        return Ok(response);
    }
}
