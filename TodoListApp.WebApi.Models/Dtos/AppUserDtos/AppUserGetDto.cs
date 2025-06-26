namespace TodoListApp.WebApi.Models.Dtos.AppUserDtos;
public class AppUserGetDto
{
    public long UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
}
