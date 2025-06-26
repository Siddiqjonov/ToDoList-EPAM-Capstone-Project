#pragma warning disable IDE0058 // Expression value is never used
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;
using TodoListApp.WebApp.Configurations;
using TodoListApp.WebApp.Middlewares;

namespace TodoListApp.WebApp;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Error handling & ProblemDetails
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        // DB configs
        builder.ConfigureUserDb();
        builder.ConfigureTodoListDb();
        builder.ConfigurationJwtAuth();

        // Auth + session + HTTP clients
        builder.Services.AddHttpContextAccessor();
        builder.Services
               .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/Auth/SignIn";
                   options.AccessDeniedPath = "/Auth/SignIn";
                   options.ExpireTimeSpan = TimeSpan.FromHours(1);
               });

        builder.Services.AddAuthorization();

        builder.Services.AddControllersWithViews();
        builder.Services.AddSession(o =>
        {
            o.Cookie.HttpOnly = true;
            o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            o.IdleTimeout = TimeSpan.FromHours(1);
        });

        builder.Services.AddHttpClient();

        builder.Services.AddHttpClient("AuthorizedClient")
               .ConfigureHttpClient((sp, client) =>
               {
                   var ctx = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
                   var token = ctx?.Request.Cookies["access_token"];
                   if (!string.IsNullOrEmpty(token))
                   {
                       client.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", token);
                   }
               });

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
