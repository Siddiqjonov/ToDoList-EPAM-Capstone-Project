#pragma warning disable SA1200 // Using directives should be placed correctly
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Core.Errors;
using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Models.Dtos.CommentDtos;
using TodoListApp.WebApi.Models.FluentValidations.CommentFluentValidators;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository commentRepository;
    private readonly IMapper mapper;

    public CommentController(ICommentRepository commentRepository, IMapper mapper)
    {
        this.commentRepository = commentRepository;
        this.mapper = mapper;
    }

    [HttpGet("getTaskComments/{taskId}")]
    public async Task<List<CommentGetDto>> GetTaskComments([FromRoute] long taskId)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access denied");

        if (taskId <= 0)
        {
            throw new InvalidArgumentException("Task id must be greater than 0");
        }

        IEnumerable<Comment> comments = await this.commentRepository.SelectCommentsByTaskIdAsync(long.Parse(userId), taskId);

        var commentDtos = this.mapper.Map<List<CommentGetDto>>(comments);
        return commentDtos;
    }

    [HttpPost("addComment")]
    public async Task<long> AddCommentToTask(CommentCreateDto commentCreateDto)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access denied");

        var validator = new CommentCreateDtoValidator();
        var result = validator.Validate(commentCreateDto);
        if (!result.IsValid)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
            throw new ValidationFailedException(errors);
        }

        var comment = this.mapper.Map<Comment>(commentCreateDto);
        comment.UserId = long.Parse(userId);
        comment.TaskItemId = commentCreateDto.TaskItemId;

        var commentId = await this.commentRepository.AddCommentToTaskAsync(comment);
        return commentId;
    }

    [HttpDelete("deleteComment/{commentId}")]
    public async Task DeleteComment([FromRoute] long commentId)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access denied");

        if (commentId <= 0)
        {
            throw new InvalidArgumentException("Comment id must be greater than 0");
        }

        await this.commentRepository.DeleteCommentByIdAsync(long.Parse(userId), commentId);
    }

    [HttpPut("updateComment")]
    public async Task UpdateComment([FromBody] CommentUpdateDto commentUpdateDto)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access denied");

        var validator = new CommentUpdateDtoValidator();
        var result = validator.Validate(commentUpdateDto);
        if (!result.IsValid)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
            throw new ValidationFailedException(errors);
        }

        var comment = this.mapper.Map<Comment>(commentUpdateDto);
        comment.UserId = long.Parse(userId);
        comment.TaskItemId = commentUpdateDto.TaskItemId;

        await this.commentRepository.UpdateCommentAsync(comment);
    }
}
