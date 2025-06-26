#pragma warning disable SA1200 // Using directives should be placed correctly
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Interfaces;

public interface ITagRepository
{
    Task<IEnumerable<Tag>> SelectAllTagsForUserAsync(long userId);

    Task<Tag> SelectTagByTagIdAsync(long userId, long tagId);

    Task<List<Tag?>> SelectTaskItemTagsAsync(int taskId);

    Task AddTagToTaskAsync(int taskId, int tagId);

    Task RemoveTagFromTaskAsync(int taskId, int tagId);

    Task<long> CreateTagAndAssignToTaskAsync(Tag tag, int taskId);
}
