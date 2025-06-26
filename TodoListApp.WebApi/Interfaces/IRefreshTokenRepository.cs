#pragma warning disable SA1200 // Using directives should be placed correctly
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Interfaces;

public interface IRefreshTokenRepository
{
    Task<long> InsertRefreshTokenAsync(RefreshToken refreshToken);

    Task<RefreshToken> SelectRefreshTokenAsync(string refreshToken, long userId);

    Task<RefreshToken?> SelectActiveTokenByUserIdAsync(long userId);

    Task RemoveRefreshTokenAsync(string token);
}
