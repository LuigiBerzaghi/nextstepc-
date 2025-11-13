using Microsoft.EntityFrameworkCore;
using NextStep.Domain.Entities;

namespace NextStep.Infrastructure.Persistence;

public class NextStepDbContext : DbContext
{
    public NextStepDbContext(DbContextOptions<NextStepDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Journey> Journeys => Set<Journey>();
    public DbSet<JourneyStep> JourneySteps => Set<JourneyStep>();
    public DbSet<ResumeAnalysis> ResumeAnalyses => Set<ResumeAnalysis>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<Profession> Professions => Set<Profession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NextStepDbContext).Assembly);
    }
}
