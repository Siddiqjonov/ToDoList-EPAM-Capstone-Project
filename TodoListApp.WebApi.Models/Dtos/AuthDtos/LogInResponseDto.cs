#pragma warning disable SA1206 // Declaration keywords should follow order
namespace TodoListApp.WebApi.Models.Dtos.AuthDtos;

public class LogInResponseDto
{
    public required string AccessToken { get; set; }

    public string RefreshToken { get; set; } = string.Empty;

    public required string TokenType { get; set; }

    public required int Expires { get; set; }
}
