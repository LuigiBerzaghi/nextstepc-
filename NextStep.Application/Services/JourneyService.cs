using System.Net;
using NextStep.Application.Common;
using NextStep.Application.DTOs.Journeys;
using NextStep.Application.Exceptions;
using NextStep.Application.Interfaces.Repositories;
using NextStep.Application.Interfaces.Services;
using NextStep.Domain.Entities;

namespace NextStep.Application.Services;

public class JourneyService : IJourneyService
{
    private readonly IJourneyRepository _journeyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public JourneyService(IJourneyRepository journeyRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _journeyRepository = journeyRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<JourneyDto> CreateJourneyAsync(int userId, CreateJourneyRequest request, CancellationToken cancellationToken)
    {
        _ = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new AppException("Usuário não encontrado.", HttpStatusCode.NotFound);

        var activeJourney = await _journeyRepository.GetActiveJourneyAsync(userId, cancellationToken);
        if (activeJourney is not null)
        {
            activeJourney.Archive();
            _journeyRepository.Update(activeJourney);
        }

        var journey = new Journey
        {
            UserId = userId,
            DesiredJob = request.DesiredJob,
            CreatedAt = DateTime.UtcNow
        };

        var steps = GenerateSteps(request).ToList();
        journey.Steps = steps;
        journey.SetTotalSteps();
        journey.RecalculateProgress();

        await _journeyRepository.AddAsync(journey, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(journey);
    }

    public async Task<JourneyDto?> GetActiveJourneyAsync(int userId, CancellationToken cancellationToken)
    {
        var journey = await _journeyRepository.GetActiveJourneyAsync(userId, cancellationToken);
        return journey is null ? null : Map(journey);
    }

    public async Task<JourneyDto?> UpdateStepProgressAsync(int userId, int stepId, UpdateJourneyStepProgressRequest request, CancellationToken cancellationToken)
    {
        var step = await _journeyRepository.GetStepByIdAsync(stepId, cancellationToken)
                   ?? throw new AppException("Etapa não encontrada.", HttpStatusCode.NotFound);

        var journey = await _journeyRepository.GetByIdAsync(step.JourneyId, cancellationToken)
                       ?? throw new AppException("Jornada não encontrada.", HttpStatusCode.NotFound);

        if (journey.UserId != userId)
        {
            throw new AppException("Etapa não pertence ao usuário.", HttpStatusCode.Forbidden);
        }

        var journeyStep = journey.Steps.FirstOrDefault(s => s.Id == stepId)
                          ?? throw new AppException("Etapa não vinculada à jornada.", HttpStatusCode.NotFound);

        journeyStep.UpdateProgress(request.Progress);
        journey.RecalculateProgress();
        _journeyRepository.Update(journey);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Map(journey);
    }

    public async Task<PagedResult<JourneyDto>> GetJourneyHistoryAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var (journeys, total) = await _journeyRepository.GetHistoryAsync(userId, pageNumber, pageSize, cancellationToken);
        var mapped = journeys.Select(Map).ToList();

        return new PagedResult<JourneyDto>
        {
            Data = mapped,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = total
        };
    }

    private static JourneyDto Map(Journey journey) =>
        new()
        {
            Id = journey.Id,
            DesiredJob = journey.DesiredJob,
            Status = journey.Status,
            OverallProgress = journey.OverallProgress,
            TotalSteps = journey.TotalSteps,
            CreatedAt = journey.CreatedAt,
            UpdatedAt = journey.UpdatedAt,
            NextStep = journey.GetNextStep() is { } next ? Map(next) : null,
            Steps = journey.Steps
                .OrderBy(s => s.Order)
                .Select(Map)
                .ToList()
        };

    private static JourneyStepDto Map(JourneyStep step) =>
        new()
        {
            Id = step.Id,
            Order = step.Order,
            Title = step.Title,
            Objective = step.Objective,
            Resources = step.Resources,
            EstimatedTime = step.EstimatedTime,
            Progress = step.Progress,
            Status = step.Status
        };

    private static IEnumerable<JourneyStep> GenerateSteps(CreateJourneyRequest request)
    {
        var order = 1;
        var steps = new List<JourneyStep>();

        foreach (var skill in request.Gaps.Any() ? request.Gaps : new[] { "Fundamentos Técnicos" })
        {
            steps.Add(new JourneyStep
            {
                Order = order++,
                Title = $"Desenvolver competência: {skill}",
                Objective = $"Consolidar fundamentos em {skill}",
                Resources = "Cursos NextStep + projetos práticos",
                EstimatedTime = "3 semanas"
            });
        }

        steps.Add(new JourneyStep
        {
            Order = order++,
            Title = "Projeto prático guiado",
            Objective = $"Aplicar conhecimentos para {request.DesiredJob}",
            Resources = "Projeto NextStep Lab",
            EstimatedTime = "2 semanas"
        });

        steps.Add(new JourneyStep
        {
            Order = order,
            Title = "Networking e mentoria",
            Objective = "Conectar-se com comunidade e mentor",
            Resources = "Mentorias NextStep, eventos FIAP",
            EstimatedTime = "1 semana"
        });

        return steps;
    }
}
