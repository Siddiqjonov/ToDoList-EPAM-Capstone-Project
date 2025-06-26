namespace TodoListApp.WebApi.Models.Models;
public class TodoList
{
    public long TodoListId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public long UserId { get; set; }

    public ApplicationUser? User { get; set; }

#pragma warning disable IDE0028 // Simplify collection initialization
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
#pragma warning restore IDE0028 // Simplify collection initialization
}
