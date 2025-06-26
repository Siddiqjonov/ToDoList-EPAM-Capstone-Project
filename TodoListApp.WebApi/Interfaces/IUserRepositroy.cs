#pragma warning disable SA1200 // Using directives should be placed correctly
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Interfaces;

public interface IUserRepositroy
{
    Task<long> InsertUserAsync(ApplicationUser user);

    Task<ApplicationUser> SelectUserByIdAsync(long userId);

    Task<ApplicationUser> SelectUserByUserNameAsync(string userName);

    Task<ApplicationUser?> SelectUserByEmailAsync(string email);

    Task UpdateUserAsync(ApplicationUser user);

    Task<long> InsertUser(ApplicationUser user);
}
