using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextStep.Domain.Entities;

namespace NextStep.Infrastructure.Persistence.Configurations;

public class ProfessionConfiguration : IEntityTypeConfiguration<Profession>
{
    public void Configure(EntityTypeBuilder<Profession> builder)
    {
        builder.ToTable("Professions");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.Title).IsRequired().HasMaxLength(160);
        builder.Property(p => p.Category).IsRequired().HasMaxLength(120);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.IsActive)
            .HasConversion(
                v => v ? "Y" : "N",
                v => v == "Y")
            .HasColumnType("CHAR(1)")
            .HasMaxLength(1);

        builder.HasData(
            new Profession
            {
                Id = 1,
                Title = "AI Product Strategist",
                Category = "Tecnologia",
                Description = "Orquestra produtos alimentados por IA com foco em ética e impacto.",
                IsActive = true
            },
            new Profession
            {
                Id = 2,
                Title = "Future of Work Consultant",
                Category = "Consultoria",
                Description = "Apoia empresas a redesenhar jornadas de talentos com automação.",
                IsActive = true
            },
            new Profession
            {
                Id = 3,
                Title = "Sustainability Data Lead",
                Category = "Dados",
                Description = "Cruza dados ESG e tendências de mercado para orientar decisões.",
                IsActive = true
            });
    }
}
