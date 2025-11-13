using System.ComponentModel.DataAnnotations;

namespace NextStep.Application.DTOs.Profile;

public class UpdateProfileRequest
{
    [MaxLength(120)]
    public string? Name { get; set; }

    [MaxLength(120)]
    public string? CurrentJob { get; set; }
    
    [EmailAddress]
    public string? NewEmail { get; set; }

    [MinLength(6)]
    public string? CurrentPassword { get; set; }

    [MinLength(6)]
    public string? NewPassword { get; set; }
}
