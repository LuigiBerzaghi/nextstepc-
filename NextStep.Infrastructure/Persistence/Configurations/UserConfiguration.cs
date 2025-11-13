using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextStep.Domain.Entities;

namespace NextStep.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Property(u => u.Email).IsRequired().HasMaxLength(160);
        builder.Property(u => u.Name).HasMaxLength(160);
        builder.Property(u => u.CurrentJob).HasMaxLength(160);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);

        builder.HasMany(u => u.Journeys)
            .WithOne()
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.ChatMessages)
            .WithOne()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
