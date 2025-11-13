using Microsoft.Extensions.DependencyInjection;
using NextStep.Application.Interfaces.Services;
using NextStep.Application.Services;

namespace NextStep.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJourneyService, JourneyService>();
        services.AddScoped<IResumeService, ResumeService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IProfessionService, ProfessionService>();
        services.AddSingleton<ITokenService, TokenService>();

        return services;
    }
}
