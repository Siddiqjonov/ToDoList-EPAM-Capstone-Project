#pragma warning disable SA1200 // Using directives should be placed correctly
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Interfaces;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> SelectCommentsByTaskIdAsync(long userId, long taskId);

    Task<long> AddCommentToTaskAsync(Comment comment);

    Task<Comment> GetCommentByIdAsync(long userId, long commentId);

    Task DeleteCommentByIdAsync(long userId, long commentId);

    Task UpdateCommentAsync(Comment comment);
}
