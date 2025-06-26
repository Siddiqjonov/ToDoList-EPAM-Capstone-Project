#pragma warning disable SA1200 // Using directives should be placed correctly
using System.Security.Claims;
using TodoListApp.WebApi.Models.Dtos.AppUserDtos;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Helpers;

public interface ITokenService
{
    public string GenerateTokent(AppUserGetDto user);

    string GenerateRefreshToken();

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
