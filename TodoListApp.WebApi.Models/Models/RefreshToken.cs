namespace TodoListApp.WebApi.Models.Models;
public class RefreshToken
{
    public long RefreshTokenId { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime Expires { get; set; }

    public bool IsRevoked { get; set; }

    public long UserId { get; set; }

    public ApplicationUser? User { get; set; }
}
