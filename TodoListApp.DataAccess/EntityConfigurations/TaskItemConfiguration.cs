#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.DataAccess.EntityConfigurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        // Primary key
        _ = builder.HasKey(t => t.Id);

        // Properties
        _ = builder.Property(t => t.Id)
        .ValueGeneratedOnAdd()
        .IsRequired();

        _ = builder.Property(t => t.Title)
        .HasMaxLength(200);

        _ = builder.Property(t => t.Description)
        .HasMaxLength(1000);

        _ = builder.Property(t => t.IsCompleted)
        .IsRequired()
        .HasDefaultValue(false);

        _ = builder.Property(t => t.DueDate);

        _ = builder.Property(t => t.TodoListId)
        .IsRequired();

        // Relationships
        _ = builder.HasOne(t => t.TodoList)
        .WithMany(tl => tl.Tasks)
        .HasForeignKey(t => t.TodoListId)
        .OnDelete(DeleteBehavior.NoAction);

        _ = builder.HasMany(t => t.Comments)
        .WithOne(c => c.TaskItem)
        .HasForeignKey(c => c.TaskItemId)
        .OnDelete(DeleteBehavior.NoAction);

        _ = builder.HasMany(t => t.TaskItemTags)
        .WithOne(tit => tit.TaskItem)
        .HasForeignKey(tit => tit.TaskItemId)
        .OnDelete(DeleteBehavior.NoAction);
    }
}
