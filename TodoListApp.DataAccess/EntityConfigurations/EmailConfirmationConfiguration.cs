#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.DataAccess.EntityConfigurations;

public class EmailConfirmationConfiguration : IEntityTypeConfiguration<EmailConfirmation>
{
    public void Configure(EntityTypeBuilder<EmailConfirmation> builder)
    {
        _ = builder.ToTable("EmailConfirmations");

        _ = builder.HasKey(ec => ec.Id);

        _ = builder.Property(ec => ec.Email)
            .IsRequired()
            .HasMaxLength(255);

        _ = builder.Property(ec => ec.Code)
            .IsRequired()
            .HasMaxLength(6);

        _ = builder.Property(ec => ec.Type)
            .IsRequired()
            .HasConversion<int>();

        _ = builder.Property(ec => ec.ExpirationTime)
            .IsRequired();

        _ = builder.Property(ec => ec.IsConfirmed)
            .IsRequired();

        // 🔍 Optional performance index
        _ = builder.HasIndex(ec => new { ec.Email, ec.Code, ec.Type });
    }
}
