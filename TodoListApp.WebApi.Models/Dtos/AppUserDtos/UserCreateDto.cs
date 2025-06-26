#pragma warning disable SA1206 // Declaration keywords should follow order

namespace TodoListApp.WebApi.Models.Dtos.AppUserDtos;

public class UserCreateDto
{
    public required string UserName { get; set; }

    public required string PhoneNumber { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }
}
