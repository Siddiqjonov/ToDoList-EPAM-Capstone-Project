#pragma warning disable IDE0058 // Expression value is never used
#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.DataAccess.EntityConfigurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Primary key
        builder.HasKey(u => u.Id);

        // Properties
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(400);

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(50);

        builder.Property(u => u.Salt)
            .IsRequired()
            .HasMaxLength(36);

        // Indexes
        builder.HasIndex(u => u.UserName)
            .IsUnique();

        builder.HasIndex(u => u.Email)
            .IsUnique();
    }
}
