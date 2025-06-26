#pragma warning disable SA1200 // Using directives should be placed correctly
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
#pragma warning restore SA1200 // Using directives should be placed correctly
#pragma warning disable format

namespace TodoListApp.WebApi.Configurations;

public static class JwtConfiguration
{
    public static void ConfigurationJwtAuth(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration.GetSection("Jwt");
        _ = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = config["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = config["Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["SecurityKey"] !)),
                };
            });
    }
}
