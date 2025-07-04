namespace TodoListApp.WebApi.Models.Dtos.TaskItemDtos;
public class TaskItemCreateDto
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public long TodoListId { get; set; }
}
