#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using TodoListApp.Core.Errors;
using TodoListApp.DataAccess;
using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Repository;

public class TagRepository : ITagRepository
{
    private readonly TodoListDbContext todoListDbContext;

    public TagRepository(TodoListDbContext todoListDbContext)
    {
        this.todoListDbContext = todoListDbContext;
    }

    public async Task<IEnumerable<Tag>> SelectAllTagsForUserAsync(long userId)
    {
        var tags = await this.todoListDbContext.TaskItemTags
            .Where(tt => tt.TaskItem.TodoList.UserId == userId)
            .Select(tt => tt.Tag)
            .Distinct()
            .ToListAsync();

        return tags;
    }

    public async Task<Tag> SelectTagByTagIdAsync(long userId, long tagId)
    {
        var tag = await this.todoListDbContext.TaskItemTags
            .Where(tt => tt.TagId == tagId && tt.TaskItem.TodoList.UserId == userId)
            .Select(tt => tt.Tag)
            .FirstOrDefaultAsync();

        return tag ?? throw new NotFoundException("Tag not found or access denied");
    }

    public async Task<List<Tag?>> SelectTaskItemTagsAsync(int taskId)
    {
        return await this.todoListDbContext.TaskItemTags
            .Where(tt => tt.TaskItemId == taskId)
            .Select(tt => (Tag?)tt.Tag)
            .ToListAsync();
    }

    public async Task AddTagToTaskAsync(int taskId, int tagId)
    {
        var exists = await this.todoListDbContext.TaskItemTags
            .AnyAsync(tt => tt.TaskItemId == taskId && tt.TagId == tagId);

        if (!exists)
        {
            _ = await this.todoListDbContext.TaskItemTags.AddAsync(new TaskItemTag
            {
                TaskItemId = taskId,
                TagId = tagId,
            });

            _ = await this.todoListDbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveTagFromTaskAsync(int taskId, int tagId)
    {
        var taskTag = await this.todoListDbContext.TaskItemTags
            .FirstOrDefaultAsync(tt => tt.TaskItemId == taskId && tt.TagId == tagId);

        if (taskTag != null)
        {
            _ = this.todoListDbContext.TaskItemTags.Remove(taskTag);
            _ = await this.todoListDbContext.SaveChangesAsync();
        }
    }

    public async Task<long> CreateTagAndAssignToTaskAsync(Tag tag, int taskId)
    {
        var existingTag = await this.todoListDbContext.Tags
            .FirstOrDefaultAsync(t => t.Name == tag.Name);

        if (existingTag == null)
        {
            _ = await this.todoListDbContext.Tags.AddAsync(tag);
            _ = await this.todoListDbContext.SaveChangesAsync();
        }
        else
        {
            tag = existingTag;
        }

        bool alreadyAssigned = await this.todoListDbContext.TaskItemTags
            .AnyAsync(t => t.TaskItemId == taskId && t.TagId == tag.TagId);

        if (!alreadyAssigned)
        {
            var taskItemTag = new TaskItemTag
            {
                TaskItemId = taskId,
                TagId = tag.TagId,
            };

            _ = await this.todoListDbContext.TaskItemTags.AddAsync(taskItemTag);
            _ = await this.todoListDbContext.SaveChangesAsync();
        }

        return tag.TagId;
    }
}
