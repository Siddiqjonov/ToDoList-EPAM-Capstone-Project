namespace TodoListApp.WebApi.Models.Dtos.ToDoListDtos;
public class ToDoListGetDto
{
    public long TodoListId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public long UserId { get; set; }
}
