namespace TodoListApp.WebApi.Models.Dtos.CommentDtos;
public class CommentUpdateDto
{
    public long CommentId { get; set; }

    public string Content { get; set; } = null!;

    public long TaskItemId { get; set; }
}
