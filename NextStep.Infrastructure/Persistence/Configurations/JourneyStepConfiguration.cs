using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextStep.Domain.Entities;

namespace NextStep.Infrastructure.Persistence.Configurations;

public class JourneyStepConfiguration : IEntityTypeConfiguration<JourneyStep>
{
    public void Configure(EntityTypeBuilder<JourneyStep> builder)
    {
        builder.ToTable("JourneySteps");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.Property(s => s.Title).IsRequired().HasMaxLength(160);
        builder.Property(s => s.Objective).HasMaxLength(500);
        builder.Property(s => s.Resources).HasMaxLength(500);
        builder.Property(s => s.EstimatedTime).HasMaxLength(60);
    }
}
