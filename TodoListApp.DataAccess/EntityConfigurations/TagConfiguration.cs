#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.DataAccess.EntityConfigurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        // Primary key
        _ = builder.HasKey(t => t.TagId);

        // Properties
        _ = builder.Property(t => t.TagId)
        .ValueGeneratedOnAdd()
        .IsRequired();

        _ = builder.Property(t => t.Name)
        .IsRequired()
        .HasMaxLength(50);

        // Relationships
        _ = builder.HasMany(t => t.TaskItemTags)
        .WithOne(tit => tit.Tag)
        .HasForeignKey(tit => tit.TagId)
        .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        _ = builder.HasIndex(t => t.Name)
        .IsUnique();
    }
}
