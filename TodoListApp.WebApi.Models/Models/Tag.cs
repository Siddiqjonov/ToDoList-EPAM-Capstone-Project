namespace TodoListApp.WebApi.Models.Models;
#pragma warning disable IDE0028 // Simplify collection initialization

public class Tag
{
    public long TagId { get; set; }

    public string Name { get; set; } = string.Empty;

    public virtual ICollection<TaskItemTag> TaskItemTags { get; set; } = new List<TaskItemTag>();
}
