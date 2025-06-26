#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using TodoListApp.Core.Errors;
using TodoListApp.DataAccess;
using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Repository;

public class TodoListRepository : ITodoListRepository
{
    private readonly TodoListDbContext todoListDbContext;

    public TodoListRepository(TodoListDbContext context)
    {
        this.todoListDbContext = context;
    }

    public async Task<TodoList> CreateTodoListAsync(TodoList todoList)
    {
        _ = await this.todoListDbContext.ToDoLists.AddAsync(todoList);
        _ = await this.todoListDbContext.SaveChangesAsync();
        return todoList;
    }

    public async Task DeleteTodoListAsync(long toDoListId, long userId)
    {
        var toDoList = await this.SelectTodoListByIdAsync(toDoListId);

        if (toDoList.UserId != userId)
        {
            throw new ForbiddenException($"To do list does not belong to user with userId: {userId}");
        }

        foreach (var task in toDoList.Tasks.ToList())
        {
            var comments = await this.todoListDbContext.Comments
                .Where(c => c.TaskItemId == task.Id)
                .ToListAsync();
            this.todoListDbContext.Comments.RemoveRange(comments);

            var tags = await this.todoListDbContext.TaskItemTags
                .Where(t => t.TaskItemId == task.Id)
                .ToListAsync();
            this.todoListDbContext.TaskItemTags.RemoveRange(tags);

            _ = this.todoListDbContext.TaskItems.Remove(task);
        }

        _ = this.todoListDbContext.ToDoLists.Remove(toDoList);
        _ = await this.todoListDbContext.SaveChangesAsync();
    }

    public async Task<TodoList> SelectTodoListByIdAsync(long toDoListId)
    {
        var toDoList = await this.todoListDbContext.ToDoLists
            .Include(t => t.Tasks)
            .FirstOrDefaultAsync(t => t.TodoListId == toDoListId);

        return toDoList ?? throw new NotFoundException($"ToDoList with id {toDoListId} not found");
    }

    public async Task<ICollection<TodoList>> SelectAllTodoListsAsync(long userId)
    {
        var toDoList = await this.todoListDbContext.ToDoLists.Where(t => t.UserId == userId).ToListAsync();
        return toDoList;
    }

    public async Task UpdateTodoListAsync(TodoList todoList)
    {
        var toDoListFromDb = await this.SelectTodoListByIdAsync(todoList.TodoListId);
        if (toDoListFromDb.UserId != todoList.UserId)
        {
            throw new ForbiddenException($"To do list does not belong to user wiht userId: {todoList.UserId}");
        }

        if (todoList == null)
        {
            throw new NotFoundException($"To do list not found to update");
        }

        if (toDoListFromDb.UserId != todoList.UserId)
        {
            throw new ForbiddenException($"Does not belong to user {todoList.UserId}");
        }

        toDoListFromDb.Name = todoList.Name;
        toDoListFromDb.Description = todoList.Description;
        _ = await this.todoListDbContext.SaveChangesAsync();
    }
}
