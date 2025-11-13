using NextStep.Application.Common;
using NextStep.Application.DTOs.Chat;

namespace NextStep.Application.Interfaces.Services;

public interface IChatService
{
    Task<ChatResponse> SendMessageAsync(int userId, ChatSendRequest request, CancellationToken cancellationToken);
    Task<PagedResult<ChatMessageDto>> GetHistoryAsync(int userId, string conversationId, int pageNumber, int pageSize, CancellationToken cancellationToken);
}
