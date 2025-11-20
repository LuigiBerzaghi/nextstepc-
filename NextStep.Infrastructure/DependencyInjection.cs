using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NextStep.Application.Interfaces.Repositories;
using NextStep.Infrastructure.Persistence;
using NextStep.Infrastructure.Repositories;

namespace NextStep.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NextStepDbContext>(options =>
            options.UseOracle(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJourneyRepository, JourneyRepository>();
        services.AddScoped<IResumeAnalysisRepository, ResumeAnalysisRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IProfessionRepository, ProfessionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
