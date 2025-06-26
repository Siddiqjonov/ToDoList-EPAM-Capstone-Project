#pragma warning disable SA1200 // Using directives should be placed correctly
using TodoListApp.WebApi.Enums;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Interfaces;

public interface ITaskItemRepository
{
    Task<List<TaskItem>> SelectTasksByTodoListIdAsync(long userId, long todoListId);

    Task<TaskItem> SelectTaskByIdAsync(long taskId, long userId);

    Task<long> InsertTaskAsync(TaskItem taskItem);

    Task DeleteTaskAsync(long taskId, long userId);

    Task UpdateTaskAsync(TaskItem taskItem);

    Task<List<TaskItem>> SelectOverDueTasksAsync(long userId);

    Task<IEnumerable<TaskItem>> SelectTasksAssignedToUserAsync(long userId, bool? isCompleted = false);

    Task<IEnumerable<TaskItem>> SortAssignedTasksByTitleOrDueDateAsync(long userId, bool? isCompleted, TaskSortBy sortBy, bool ascending = true);

    Task UpdateTaskStatusAsync(long taskId, long userId, bool isCompleted);

    Task<IEnumerable<TaskItem>> SearchTasksAsync(long userId, string? title, DateTime? createdDate, DateTime? dueDate);

    Task<TaskItem> SelectTaskDetailsAsync(long userId, long taskId);

    Task<IEnumerable<TaskItem>> SelectTasksByTagIdAsync(long userId, long tagId);

    Task<bool> AddTagToTaskAsync(long userId, long taskId, long tagId);

    Task RemoveTagFromTaskAsync(long userId, long taskId, long tagId);
}
