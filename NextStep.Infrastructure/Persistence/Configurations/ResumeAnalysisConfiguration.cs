using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextStep.Domain.Entities;

namespace NextStep.Infrastructure.Persistence.Configurations;

public class ResumeAnalysisConfiguration : IEntityTypeConfiguration<ResumeAnalysis>
{
    public void Configure(EntityTypeBuilder<ResumeAnalysis> builder)
    {
        builder.ToTable("ResumeAnalyses");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();

        builder.Property(r => r.SummaryJson).IsRequired();
    }
}
