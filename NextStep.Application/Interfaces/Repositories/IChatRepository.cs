using NextStep.Domain.Entities;

namespace NextStep.Application.Interfaces.Repositories;

public interface IChatRepository
{
    Task AddAsync(ChatMessage chatMessage, CancellationToken cancellationToken);
    Task<(IReadOnlyCollection<ChatMessage> Messages, int TotalCount)> GetHistoryAsync(
        int userId,
        string conversationId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}
