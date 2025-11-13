using NextStep.Application.Common;
using NextStep.Application.DTOs.Chat;
using NextStep.Application.Interfaces.Repositories;
using NextStep.Application.Interfaces.Services;
using NextStep.Domain.Entities;
using NextStep.Domain.Enums;

namespace NextStep.Application.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChatService(IChatRepository chatRepository, IUnitOfWork unitOfWork)
    {
        _chatRepository = chatRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ChatResponse> SendMessageAsync(int userId, ChatSendRequest request, CancellationToken cancellationToken)
    {
        var userMessage = new ChatMessage
        {
            UserId = userId,
            ConversationId = request.ConversationId,
            Role = ChatRole.User,
            Message = request.Message,
            Timestamp = DateTime.UtcNow
        };

        await _chatRepository.AddAsync(userMessage, cancellationToken);

        var aiMessage = new ChatMessage
        {
            UserId = userId,
            ConversationId = request.ConversationId,
            Role = ChatRole.Ai,
            Message = GenerateAiResponse(request.Message),
            Timestamp = DateTime.UtcNow
        };

        await _chatRepository.AddAsync(aiMessage, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ChatResponse
        {
            Reply = Map(aiMessage)
        };
    }

    public async Task<PagedResult<ChatMessageDto>> GetHistoryAsync(int userId, string conversationId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var (messages, total) = await _chatRepository.GetHistoryAsync(userId, conversationId, pageNumber, pageSize, cancellationToken);
        var data = messages
            .OrderBy(m => m.Timestamp)
            .Select(Map)
            .ToList();

        return new PagedResult<ChatMessageDto>
        {
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = total
        };
    }

    private static ChatMessageDto Map(ChatMessage message) =>
        new()
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            Role = message.Role,
            Message = message.Message,
            Timestamp = message.Timestamp
        };

    private static string GenerateAiResponse(string userMessage)
    {
        if (userMessage.Contains("carreira", StringComparison.OrdinalIgnoreCase))
        {
            return "Estou analisando seu momento e recomendo explorar produtos digitais com foco em IA. Que tipo de projeto mais empolga você agora?";
        }

        if (userMessage.Contains("curriculo", StringComparison.OrdinalIgnoreCase) ||
            userMessage.Contains("currículo", StringComparison.OrdinalIgnoreCase))
        {
            return "Revise seu currículo destacando resultados mensuráveis e inclua habilidades emergentes como automação e orquestração de IA.";
        }

        if (userMessage.Contains("ansioso", StringComparison.OrdinalIgnoreCase))
        {
            return "Respire fundo. Divida a jornada em pequenos experimentos semanais e celebre cada aprendizado.";
        }

        return "Excelente pergunta! Foque em criar portfólio alinhado ao Futuro do Trabalho, combinando tecnologia, human skills e impacto sustentável.";
    }
}
