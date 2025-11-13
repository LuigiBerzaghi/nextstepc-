using Microsoft.EntityFrameworkCore;
using NextStep.Application.Interfaces.Repositories;
using NextStep.Domain.Entities;
using NextStep.Infrastructure.Persistence;

namespace NextStep.Infrastructure.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly NextStepDbContext _context;

    public ChatRepository(NextStepDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(ChatMessage chatMessage, CancellationToken cancellationToken) =>
        _context.ChatMessages.AddAsync(chatMessage, cancellationToken).AsTask();

    public async Task<(IReadOnlyCollection<ChatMessage> Messages, int TotalCount)> GetHistoryAsync(int userId, string conversationId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.ChatMessages
            .Where(m => m.UserId == userId && m.ConversationId == conversationId)
            .OrderByDescending(m => m.Timestamp);

        var total = await query.CountAsync(cancellationToken);
        var messages = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (messages, total);
    }
}
