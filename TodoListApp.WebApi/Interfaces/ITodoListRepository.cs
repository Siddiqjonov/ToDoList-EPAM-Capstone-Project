#pragma warning disable SA1200 // Using directives should be placed correctly
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Interfaces;

public interface ITodoListRepository
{
    Task<ICollection<TodoList>> SelectAllTodoListsAsync(long userId);

    Task<TodoList> CreateTodoListAsync(TodoList todoList);

    Task<TodoList> SelectTodoListByIdAsync(long toDoListId);

    Task DeleteTodoListAsync(long toDoListId, long userId);

    Task UpdateTodoListAsync(TodoList todoList);
}
