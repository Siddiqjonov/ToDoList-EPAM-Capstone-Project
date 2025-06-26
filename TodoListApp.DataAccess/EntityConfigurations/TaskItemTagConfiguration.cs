#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.DataAccess.EntityConfigurations;

public class TaskItemTagConfiguration : IEntityTypeConfiguration<TaskItemTag>
{
    public void Configure(EntityTypeBuilder<TaskItemTag> builder)
    {
        // Primary key
        _ = builder.HasKey(tit => tit.TaskItemTagId);

        _ = builder.HasIndex(t => new { t.TaskItemId, t.TagId }).IsUnique();

        // Relationships
        _ = builder.HasOne(tit => tit.TaskItem)
        .WithMany(ti => ti.TaskItemTags)
        .HasForeignKey(tit => tit.TaskItemId)
        .OnDelete(DeleteBehavior.NoAction);

        _ = builder.HasOne(tit => tit.Tag)
        .WithMany(t => t.TaskItemTags)
        .HasForeignKey(tit => tit.TagId)
        .OnDelete(DeleteBehavior.NoAction);
    }
}
