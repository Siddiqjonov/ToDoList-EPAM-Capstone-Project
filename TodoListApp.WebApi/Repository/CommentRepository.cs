#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore;
using TodoListApp.Core.Errors;
using TodoListApp.DataAccess;
using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly TodoListDbContext todoListDbContext;

    public CommentRepository(TodoListDbContext todoListDbContext)
    {
        this.todoListDbContext = todoListDbContext;
    }

    public async Task<long> AddCommentToTaskAsync(Comment comment)
    {
        _ = await this.todoListDbContext.Comments.AddAsync(comment);
        _ = await this.todoListDbContext.SaveChangesAsync();
        return comment.CommentId;
    }

    public async Task<IEnumerable<Comment>> SelectCommentsByTaskIdAsync(long userId, long taskId)
    {
        var taskItem = await this.todoListDbContext.TaskItems
            .Include(t => t.Comments)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.TodoList.UserId == userId)
            ?? throw new NotFoundException("Task not found");

        return taskItem.Comments;
    }

    public async Task DeleteCommentByIdAsync(long userId, long commentId)
    {
        var comment = await this.GetCommentByIdAsync(userId, commentId);

        _ = this.todoListDbContext.Comments.Remove(comment);
        _ = await this.todoListDbContext.SaveChangesAsync();
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        var commentFromDb = await this.GetCommentByIdAsync(comment.UserId, comment.CommentId);
        commentFromDb.Content = comment.Content;
        _ = await this.todoListDbContext.SaveChangesAsync();
    }

    public async Task<Comment> GetCommentByIdAsync(long userId, long commentId)
    {
        var comment = await this.todoListDbContext.Comments
            .Include(c => c.TaskItem)
            .ThenInclude(t => t.TodoList)
            .FirstOrDefaultAsync(c => c.CommentId == commentId && c.TaskItem.TodoList != null && c.TaskItem.TodoList.UserId == userId)
            ?? throw new NotFoundException($"Comment with ID {commentId} not found or access denied");

        return comment;
    }
}
