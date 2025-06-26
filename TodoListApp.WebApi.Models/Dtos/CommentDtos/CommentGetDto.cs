namespace TodoListApp.WebApi.Models.Dtos.CommentDtos;
public class CommentGetDto
{
    public long CommentId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public long UserId { get; set; }

    public long TaskItemId { get; set; }
}
