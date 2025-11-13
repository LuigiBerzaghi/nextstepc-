using System.Net;
using NextStep.Application.DTOs.Dashboard;
using NextStep.Application.DTOs.Journeys;
using NextStep.Application.Exceptions;
using NextStep.Application.Interfaces.Repositories;
using NextStep.Application.Interfaces.Services;

namespace NextStep.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IUserRepository _userRepository;

    public DashboardService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<DashboardResponse> GetDashboardAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithJourneysAsync(userId, cancellationToken)
                   ?? throw new AppException("Usuário não encontrado.", HttpStatusCode.NotFound);

        var journeys = user.Journeys.ToList();
        var total = journeys.Count;
        var completed = journeys.Count(j => j.Status == Domain.Enums.JourneyStatus.Completed);
        var average = total == 0 ? 0 : Math.Round(journeys.Average(j => j.OverallProgress), 1);
        var nextStep = user.GetActiveJourney()?.GetNextStep();

        return new DashboardResponse
        {
            Email = user.Email,
            Name = user.Name,
            CurrentJob = user.CurrentJob,
            TotalJourneys = total,
            CompletedJourneys = completed,
            AverageProgress = average,
            NextStep = nextStep is null ? null : new JourneyStepDto
            {
                Id = nextStep.Id,
                Order = nextStep.Order,
                Title = nextStep.Title,
                Objective = nextStep.Objective,
                Resources = nextStep.Resources,
                EstimatedTime = nextStep.EstimatedTime,
                Progress = nextStep.Progress,
                Status = nextStep.Status
            }
        };
    }
}
