using NextStep.Domain.Enums;

namespace NextStep.Application.DTOs.Chat;

public class ChatMessageDto
{
    public int Id { get; init; }
    public string ConversationId { get; init; } = string.Empty;
    public ChatRole Role { get; init; }
    public string Message { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}
