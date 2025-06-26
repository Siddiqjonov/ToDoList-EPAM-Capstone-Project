#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.AspNetCore.Identity;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Models.Models;

public class ApplicationUser : IdentityUser<long>
{
    public string Salt { get; set; } = string.Empty;
}
