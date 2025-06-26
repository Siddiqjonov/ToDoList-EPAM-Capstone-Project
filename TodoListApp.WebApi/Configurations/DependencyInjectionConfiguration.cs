#pragma warning disable SA1200 // Using directives should be placed correctly
using TodoListApp.AutoMapperProfile.AutoMapper;
using TodoListApp.WebApi.AuthService;
using TodoListApp.WebApi.Helpers;
using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Repository;
#pragma warning restore SA1200 // Using directives should be placed correctly
#pragma warning disable IDE0058 // Expression value is never used

namespace TodoListApp.WebApi.Configurations;

public static class DependencyInjectionConfiguration
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthService, AuthServicee>();
        builder.Services.AddScoped<ITodoListRepository, TodoListRepository>();
        builder.Services.AddScoped<ITagRepository, TagRepository>();
        builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<IUserRepositroy, UserRepositroy>();
        builder.Services.AddScoped<IEmailSender, EmailSender>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }

    public static void RegisterAutoMapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(ToDoListProfile));
        builder.Services.AddAutoMapper(typeof(UserProfile));
        builder.Services.AddAutoMapper(typeof(TaskItemProfile));
        builder.Services.AddAutoMapper(typeof(CommentProfile));
        builder.Services.AddAutoMapper(typeof(TagProfile));
    }
}
