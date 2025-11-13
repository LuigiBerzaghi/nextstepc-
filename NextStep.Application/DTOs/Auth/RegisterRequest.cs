using System.ComponentModel.DataAnnotations;

namespace NextStep.Application.DTOs.Auth;

public class RegisterRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(120)]
    public string? Name { get; set; }

    [MaxLength(120)]
    public string? CurrentJob { get; set; }
}
