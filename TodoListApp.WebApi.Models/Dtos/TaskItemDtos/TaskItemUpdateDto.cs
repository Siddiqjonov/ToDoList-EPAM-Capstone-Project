namespace TodoListApp.WebApi.Models.Dtos.TaskItemDtos;
public class TaskItemUpdateDto
{
    public long TaskItemId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? DueDate { get; set; }

    public long TodoListId { get; set; }
}
