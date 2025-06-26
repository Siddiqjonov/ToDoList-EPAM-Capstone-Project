#pragma warning disable SA1206 // Declaration keywords should follow order
#pragma warning disable IDE0028 // Simplify collection initialization

namespace TodoListApp.WebApi.Models.Models;
public class TaskItem
{
    public long Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? DueDate { get; set; }

    public bool IsOverdue => this.DueDate.HasValue && this.DueDate < DateTime.UtcNow && !this.IsCompleted;

    public long TodoListId { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public required TodoList TodoList { get; set; }

    public ICollection<TaskItemTag> TaskItemTags { get; set; } = new List<TaskItemTag>();

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
