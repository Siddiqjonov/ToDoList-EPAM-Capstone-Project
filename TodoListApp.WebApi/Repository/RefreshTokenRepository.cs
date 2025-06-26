#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using TodoListApp.Core.Errors;
using TodoListApp.DataAccess;
using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Repository;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly TodoListDbContext todoListDbContext;

    public RefreshTokenRepository(TodoListDbContext todoListDbContext)
    {
        this.todoListDbContext = todoListDbContext;
    }

    public async Task<long> InsertRefreshTokenAsync(RefreshToken refreshToken)
    {
        _ = await this.todoListDbContext.RefreshTokens.AddAsync(refreshToken);
        _ = await this.todoListDbContext.SaveChangesAsync();
        return refreshToken.RefreshTokenId;
    }

    public async Task<RefreshToken?> SelectActiveTokenByUserIdAsync(long userId)
    {
        RefreshToken? refreshToke;
        try
        {
            refreshToke = await this.todoListDbContext.RefreshTokens
            .Include(rf => rf.User)
            .SingleOrDefaultAsync(rf => rf.UserId == userId && !rf.IsRevoked && rf.Expires > DateTime.UtcNow);
        }
        catch (InvalidOperationException ex)
        {
            throw new DuplicateEntryException($"2 or more active refreshToken found with userId: {userId} found!\nAnd {ex.Message}");
        }

        return refreshToke;
    }

    public async Task<RefreshToken> SelectRefreshTokenAsync(string refreshToken, long userId)
    {
        var refToken = await this.todoListDbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.UserId == userId);

        return refToken ?? throw new InvalidArgumentException($"RefreshToken with {userId} is invalid");
    }

    public async Task RemoveRefreshTokenAsync(string token)
    {
        var refreshToken = await this.todoListDbContext.RefreshTokens.FirstOrDefaultAsync(rf => rf.Token == token)
            ?? throw new EntityNotFoundException($"Refresh token: {token} not found");

        _ = this.todoListDbContext.RefreshTokens.Remove(refreshToken);
        _ = await this.todoListDbContext.SaveChangesAsync();
    }
}
