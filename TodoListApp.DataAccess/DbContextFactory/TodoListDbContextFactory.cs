#pragma warning disable IDE0058 // Expression value is never used
#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.DataAccess.DbContextFactory;

public class TodoListDbContextFactory : IDesignTimeDbContextFactory<TodoListDbContext>
{
    public TodoListDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "TodoListApp.WebApi");

        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<TodoListDbContext>();
        optionsBuilder.UseSqlServer(config.GetConnectionString("TodoListDefaultConnection"));

        return new TodoListDbContext(optionsBuilder.Options);
    }
}
