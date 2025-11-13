using System.ComponentModel.DataAnnotations;

namespace NextStep.Application.DTOs.Chat;

public class ChatSendRequest
{
    [Required]
    public string ConversationId { get; set; } = string.Empty;

    [Required, MinLength(2)]
    public string Message { get; set; } = string.Empty;
}
