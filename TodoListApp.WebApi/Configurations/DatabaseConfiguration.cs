#pragma warning disable IDE0058 // Expression value is never used

#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoListApp.DataAccess;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Configurations;

public static class DatabaseConfiguration
{
    public static void ConfigureTodoListDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("TodoListDefaultConnection");
        builder.Services.AddDbContext<TodoListDbContext>(options => options.UseSqlServer(connectionString));
    }

    public static void ConfigureUserDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("UserDefaultConnection");
        builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole<long>>()
                        .AddEntityFrameworkStores<UserDbContext>()
                        .AddDefaultTokenProviders();
    }
}
