#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly
#pragma warning disable IDE0058 // Expression value is never used

namespace TodoListApp.DataAccess.EntityConfigurations;
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // Primary Key
        builder.HasKey(rt => rt.RefreshTokenId);

        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(rt => rt.Expires)
            .IsRequired();

        // IsRevoked - required
        builder.Property(rt => rt.IsRevoked)
            .IsRequired();

        // UserId - foreign key
        builder.Property(rt => rt.UserId)
            .IsRequired();

        // Navigation property (optional: on delete behavior, etc.)
        builder.HasOne(rt => rt.User)
            .WithMany() // or .WithMany(u => u.RefreshTokens) if you have that navigation on ApplicationUser
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade); // or Restrict depending on your use case
    }
}
