#pragma warning disable IDE0058 // Expression value is never used
#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.DataAccess.EntityConfigurations;

public class ToDoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        // Primary key
        builder.HasKey(t => t.TodoListId);

        // Properties
        builder.Property(t => t.TodoListId)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.UserId)
            .IsRequired();

        // Relationships within the same DbContext
        builder.HasMany(t => t.Tasks)
            .WithOne(ti => ti.TodoList)
            .HasForeignKey(t => t.TodoListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
