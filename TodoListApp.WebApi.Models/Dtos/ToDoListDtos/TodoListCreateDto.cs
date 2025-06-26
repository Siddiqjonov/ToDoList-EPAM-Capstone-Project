namespace TodoListApp.WebApi.Models.Dtos.ToDoListDtos;
public class TodoListCreateDto
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
