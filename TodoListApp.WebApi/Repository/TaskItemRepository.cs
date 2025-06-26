#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using TodoListApp.Core.Errors;
using TodoListApp.DataAccess;
using TodoListApp.WebApi.Enums;
using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Repository;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly TodoListDbContext todoListDbContext;

    public TaskItemRepository(TodoListDbContext todoListDbContext)
    {
        this.todoListDbContext = todoListDbContext;
    }

    public async Task DeleteTaskAsync(long taskId, long userId)
    {
        var taskItem = await this.SelectTaskByIdAsync(taskId, userId);

        var tags = await this.todoListDbContext.TaskItemTags
            .Where(t => t.TaskItemId == taskItem.Id)
            .ToListAsync();
        this.todoListDbContext.TaskItemTags.RemoveRange(tags);

        var comments = await this.todoListDbContext.Comments
            .Where(c => c.TaskItemId == taskItem.Id)
            .ToListAsync();
        this.todoListDbContext.Comments.RemoveRange(comments);

        _ = this.todoListDbContext.TaskItems.Remove(taskItem);
        _ = await this.todoListDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<TaskItem>> SelectTasksAssignedToUserAsync(long userId, bool? isCompleted = false)
    {
        var query = this.todoListDbContext.TaskItems
            .Where(t => t.TodoList != null && t.TodoList.UserId == userId);

        if (isCompleted != null)
        {
            query = query.Where(t => t.IsCompleted == isCompleted);
        }

        return await query.ToListAsync();
    }

    public async Task<long> InsertTaskAsync(TaskItem taskItem)
    {
        _ = await this.todoListDbContext.TaskItems.AddAsync(taskItem);
        _ = await this.todoListDbContext.SaveChangesAsync();
        return taskItem.Id;
    }

    public async Task<List<TaskItem>> SelectOverDueTasksAsync(long userId)
    {
        var taskItems = await this.todoListDbContext.TaskItems
            .Include(t => t.TodoList)
            .Where(t => t.IsOverdue && t.TodoList.UserId == userId)
            .ToListAsync();

        return taskItems;
    }

    public async Task<TaskItem> SelectTaskByIdAsync(long taskId, long userId)
    {
        var taskItem = await this.todoListDbContext.TaskItems
            .Include(t => t.TodoList)
            .Include(t => t.TaskItemTags)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.TodoList != null && t.TodoList.UserId == userId);

        return taskItem
            ?? throw new EntityNotFoundException($"Task with task id: {taskId} not found or it does not belong to user with userId: {userId}");
    }

    public async Task<List<TaskItem>> SelectTasksByTodoListIdAsync(long userId, long todoListId)
    {
        List<TaskItem> taskItems = await this.todoListDbContext.TaskItems.Include(t => t.TodoList)
            .Where(t => t.TodoList != null && t.TodoListId == todoListId && t.TodoList.UserId == userId)
        .ToListAsync();

        return taskItems;
    }

    public async Task UpdateTaskAsync(TaskItem taskItem)
    {
        _ = this.todoListDbContext.TaskItems.Update(taskItem);
        _ = await this.todoListDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<TaskItem>> SortAssignedTasksByTitleOrDueDateAsync(long userId, bool? isCompleted, TaskSortBy sortBy, bool ascending = true)
    {
        var query = this.todoListDbContext.TaskItems.Where(t => t.TodoList != null && t.TodoList.UserId == userId);

        if (isCompleted != null)
        {
            query = query.Where(t => t.IsCompleted == isCompleted);
        }

        query = (sortBy, ascending) switch
        {
            (TaskSortBy.Title, true) => query.OrderBy(t => t.Title),
            (TaskSortBy.Title, false) => query.OrderByDescending(t => t.Title),
            (TaskSortBy.DueDate, true) => query.OrderBy(t => t.DueDate),
            (TaskSortBy.DueDate, false) => query.OrderByDescending(t => t.DueDate),
            _ => query
        };

        return await query.ToListAsync();
    }

    public async Task UpdateTaskStatusAsync(long taskId, long userId, bool isCompleted)
    {
        var taskItem = await this.SelectTaskByIdAsync(taskId, userId);
        taskItem.IsCompleted = isCompleted;
        await this.UpdateTaskAsync(taskItem);
    }

    public async Task<IEnumerable<TaskItem>> SearchTasksAsync(long userId, string? title, DateTime? createdDate, DateTime? dueDate)
    {
        var query = this.todoListDbContext.TaskItems.Where(t => t.TodoList != null && t.TodoList.UserId == userId);

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(t => t.Title != null && t.Title.Contains(title));
        }

        if (createdDate.HasValue)
        {
            query = query.Where(t => t.CreatedDate.Date == createdDate.Value.Date);
        }

        if (dueDate.HasValue)
        {
            query = query.Where(t => t.DueDate != null && t.DueDate.Value.Date == dueDate.Value.Date);
        }

        return await query.ToListAsync();
    }

    public async Task<TaskItem> SelectTaskDetailsAsync(long userId, long taskId)
    {
        var taskItem = await this.todoListDbContext.TaskItems
        .Include(t => t.TodoList)
        .Include(t => t.TaskItemTags)
            .ThenInclude(tt => tt.Tag)
        .FirstOrDefaultAsync(t => t.Id == taskId && t.TodoList != null && t.TodoList.UserId == userId)
        ?? throw new NotFoundException("Task not found or access denied");

        return taskItem;
    }

    public async Task<IEnumerable<TaskItem>> SelectTasksByTagIdAsync(long userId, long tagId)
    {
        var taskItems = await this.todoListDbContext.TaskItems
            .Where(t => t.TodoList != null && t.TodoList.UserId == userId && t.TaskItemTags.Any(tt => tt.TagId == tagId))
            .ToListAsync();

        return taskItems;
    }

    public async Task<bool> AddTagToTaskAsync(long userId, long taskId, long tagId)
    {
        var taskItem = await this.SelectTaskByIdAsync(taskId, userId);

        var addedTagCount = 0;
        var alreadyTagged = taskItem.TaskItemTags.Any(tt => tt.TagId == tagId);
        if (!alreadyTagged)
        {
            taskItem.TaskItemTags.Add(new TaskItemTag
            {
                TaskItemId = taskId,
                TagId = tagId,
            });

            addedTagCount = await this.todoListDbContext.SaveChangesAsync();
        }

        return addedTagCount > 0;
    }

    public async Task RemoveTagFromTaskAsync(long userId, long taskId, long tagId)
    {
        var taskItem = await this.SelectTaskByIdAsync(taskId, userId);

        var taskItemTag = taskItem.TaskItemTags
            .FirstOrDefault(t => t.TaskItemId == taskId && t.TagId == tagId)
            ?? throw new NotFoundException("Tag not associated with this task");

        _ = taskItem.TaskItemTags.Remove(taskItemTag);
        _ = await this.todoListDbContext.SaveChangesAsync();
    }
}
