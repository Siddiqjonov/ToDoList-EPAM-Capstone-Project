namespace TodoListApp.WebApi.Models.Models;
public class EmailConfirmation
{
    public int Id { get; set; }

    public string Email { get; set; } = default!;

    public string Code { get; set; } = default!;

    public ConfirmationType Type { get; set; }

    public DateTime ExpirationTime { get; set; } = DateTime.UtcNow.AddMinutes(60);

    public bool IsConfirmed { get; set; } = false;
}

public enum ConfirmationType
{
    EmailVerification,
    PasswordReset,
}
