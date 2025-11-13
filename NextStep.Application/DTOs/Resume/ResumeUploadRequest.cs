using System.ComponentModel.DataAnnotations;

namespace NextStep.Application.DTOs.Resume;

public class ResumeUploadRequest
{
    [Required, MinLength(10)]
    public string ResumeText { get; set; } = string.Empty;
}
