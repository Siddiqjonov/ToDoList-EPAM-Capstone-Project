namespace TodoListApp.WebApi.Models.Dtos.CommentDtos;
public class CommentCreateDto
{
    public string Content { get; set; } = null!;

    public long TaskItemId { get; set; }
}
