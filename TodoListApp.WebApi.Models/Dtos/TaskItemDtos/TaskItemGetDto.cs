#pragma warning disable SA1200 // Using directives should be placed correctly
using System.Text.Json.Serialization;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Models.Dtos.TaskItemDtos;
public class TaskItemGetDto
{
    public long TaskItemId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public bool IsOverdue => this.DueDate < DateTime.UtcNow && !this.IsCompleted;

    public long TodoListId { get; set; }
}
