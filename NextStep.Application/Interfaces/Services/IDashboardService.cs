using NextStep.Application.DTOs.Dashboard;

namespace NextStep.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<DashboardResponse> GetDashboardAsync(int userId, CancellationToken cancellationToken);
}
