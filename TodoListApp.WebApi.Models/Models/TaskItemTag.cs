namespace TodoListApp.WebApi.Models.Models;

public class TaskItemTag
{
    public long TaskItemTagId { get; set; }

    public long TaskItemId { get; set; }

    public TaskItem TaskItem { get; set; } = null!;

    public long TagId { get; set; }

    public Tag Tag { get; set; } = null!;
}
