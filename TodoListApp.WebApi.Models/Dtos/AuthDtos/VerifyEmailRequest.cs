namespace TodoListApp.WebApi.Models.Dtos.AuthDtos;
public class VerifyEmailRequest
{
    public string Email { get; set; } = default!;

    public string Code { get; set; } = default!;
}
