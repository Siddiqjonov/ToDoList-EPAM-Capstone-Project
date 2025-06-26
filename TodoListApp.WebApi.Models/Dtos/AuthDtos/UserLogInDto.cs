#pragma warning disable SA1206 // Declaration keywords should follow order

namespace TodoListApp.WebApi.Models.Dtos.AuthDtos;

public class UserLogInDto
{
    public required string UserName { get; set; }

    public required string Password { get; set; }
}
