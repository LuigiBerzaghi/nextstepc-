using System.ComponentModel.DataAnnotations;

namespace NextStep.Application.DTOs.Journeys;

public class UpdateJourneyStepProgressRequest
{
    [Range(0, 100)]
    public int Progress { get; set; }
}
