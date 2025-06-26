#pragma warning disable IDE0058 // Expression value is never used
#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using TodoListApp.DataAccess.EntityConfigurations;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.DataAccess;

public class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> TaskItems { get; set; } = null!;

    public DbSet<ApplicationUser> Users { get; set; } = null!;

    public DbSet<TodoList> ToDoLists { get; set; } = null!;

    public DbSet<Comment> Comments { get; set; } = null!;

    public DbSet<Tag> Tags { get; set; } = null!;

    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    public DbSet<TaskItemTag> TaskItemTags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new ToDoListConfiguration());
        modelBuilder.ApplyConfiguration(new TaskItemConfiguration());
        modelBuilder.ApplyConfiguration(new TaskItemTagConfiguration());
    }
}
