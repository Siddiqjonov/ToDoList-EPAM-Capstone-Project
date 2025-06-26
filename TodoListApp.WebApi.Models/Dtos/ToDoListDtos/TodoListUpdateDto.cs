namespace TodoListApp.WebApi.Models.Dtos.ToDoListDtos;
public class TodoListUpdateDto
{
    public long TodoListId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public long UserId { get; set; }
}
