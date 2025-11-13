using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextStep.Domain.Entities;

namespace NextStep.Infrastructure.Persistence.Configurations;

public class JourneyConfiguration : IEntityTypeConfiguration<Journey>
{
    public void Configure(EntityTypeBuilder<Journey> builder)
    {
        builder.ToTable("Journeys");

        builder.HasKey(j => j.Id);
        builder.Property(j => j.Id).ValueGeneratedOnAdd();

        builder.Property(j => j.DesiredJob)
            .IsRequired()
            .HasMaxLength(160);

        builder.Property(j => j.OverallProgress).HasDefaultValue(0);
        builder.Property(j => j.TotalSteps).HasDefaultValue(0);

        builder.HasMany(j => j.Steps)
            .WithOne()
            .HasForeignKey(s => s.JourneyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
