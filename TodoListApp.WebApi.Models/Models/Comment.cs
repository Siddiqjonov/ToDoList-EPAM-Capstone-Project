namespace TodoListApp.WebApi.Models.Models;

public class Comment
{
    public long CommentId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public long UserId { get; set; }

    public ApplicationUser? User { get; set; }

    public long TaskItemId { get; set; }

    public TaskItem TaskItem { get; set; } = null!;
}
