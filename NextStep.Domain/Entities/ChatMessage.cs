using NextStep.Domain.Enums;

namespace NextStep.Domain.Entities;

public class ChatMessage
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ConversationId { get; set; } = string.Empty;
    public ChatRole Role { get; set; } = ChatRole.User;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
