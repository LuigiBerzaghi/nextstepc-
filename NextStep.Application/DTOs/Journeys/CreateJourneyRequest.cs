using System.ComponentModel.DataAnnotations;

namespace NextStep.Application.DTOs.Journeys;

public class CreateJourneyRequest
{
    [Required, MaxLength(120)]
    public string DesiredJob { get; set; } = string.Empty;

    public ICollection<string> CurrentSkills { get; set; } = new List<string>();

    public ICollection<string> Gaps { get; set; } = new List<string>();
}
