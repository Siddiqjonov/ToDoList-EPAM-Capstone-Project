#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using TodoListApp.Core.Errors;
using TodoListApp.DataAccess;
using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Repository;

public class UserRepositroy : IUserRepositroy
{
    private readonly UserDbContext userDbContext;
    private readonly TodoListDbContext todoListDbContext;

    public UserRepositroy(UserDbContext userDbContext, TodoListDbContext todoListDbContext)
    {
        this.userDbContext = userDbContext;
        this.todoListDbContext = todoListDbContext;
    }

    public async Task<long> InsertUserAsync(ApplicationUser user)
    {
        _ = await this.userDbContext.Users.AddAsync(user);
        _ = await this.userDbContext.SaveChangesAsync();
        return user.Id;
    }

    public async Task<ApplicationUser> SelectUserByIdAsync(long userId)
    {
        var user = await this.todoListDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return user ?? throw new EntityNotFoundException($"User with userId {userId} not found");
    }

    public async Task<ApplicationUser> SelectUserByUserNameAsync(string userName)
    {
        var user = await this.todoListDbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        return user ?? throw new EntityNotFoundException($"User with {userName} not found");
    }

    public async Task<ApplicationUser?> SelectUserByEmailAsync(string email)
    {
        return await this.userDbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task UpdateUserAsync(ApplicationUser user)
    {
        _ = this.userDbContext.Users.Update(user);
        _ = this.todoListDbContext.Users.Update(user);
        _ = await this.userDbContext.SaveChangesAsync();
        _ = await this.todoListDbContext.SaveChangesAsync();
    }

    public async Task<long> InsertUser(ApplicationUser user)
    {
        var sql = @"
                INSERT INTO dbo.Users ( Salt, UserName, NormalizedUserName, Email, NormalizedEmail,
                    EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber,
                    PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount )
                    VALUES ( {0}, {1}, UPPER({1}), {2}, UPPER({2}), 0, {3}, NEWID(),
                    NEWID(), {4}, 0, 0, NULL, 0, 0 ); ";

        return await this.todoListDbContext.Database.ExecuteSqlRawAsync(
            sql, user.Salt!, user.UserName!, user.Email!, user.PasswordHash!, user.PhoneNumber!);
    }
}
