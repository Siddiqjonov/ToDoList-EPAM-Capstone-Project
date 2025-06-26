#pragma warning disable SA1200 // Using directives should be placed correctly
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Core.Errors;
using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Models.Dtos.ToDoListDtos;
using TodoListApp.WebApi.Models.FluentValidations.ToDoListFluentValidators;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/toDoList")]
[ApiController]
public class TodoListController : ControllerBase
{
    private readonly ITodoListRepository todoListRepository;
    private readonly IMapper mapper;

    public TodoListController(ITodoListRepository todoListRepository, IMapper mapper)
    {
        this.todoListRepository = todoListRepository;
        this.mapper = mapper;
    }

    [HttpGet("getAll")]
    public async Task<List<ToDoListGetDto>> GetAllToDoLists()
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access forbidden");

        var toDoLists = await this.todoListRepository.SelectAllTodoListsAsync(long.Parse(userId));
        var toDoListDtos = this.mapper.Map<List<ToDoListGetDto>>(toDoLists);
        return toDoListDtos;
    }

    [HttpPost("add")]
    public async Task<long> AddToDoList(TodoListCreateDto todoListCreateDto)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access forbidden");

        var validator = new TodoListCreateDtoValidator();
        var result = validator.Validate(todoListCreateDto);

        if (!result.IsValid)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
            throw new ValidationFailedException(errors);
        }

        var toDoList = this.mapper.Map<TodoList>(todoListCreateDto);
        toDoList.UserId = long.Parse(userId);
        toDoList.CreatedDate = DateTime.UtcNow;

        var createdToDoList = await this.todoListRepository.CreateTodoListAsync(toDoList);
        return createdToDoList.TodoListId;
    }

    [HttpDelete("delete/{id}")]
    public async Task DeleteToDoList(long id)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access forbidden");

        await this.todoListRepository.DeleteTodoListAsync(id, long.Parse(userId));
    }

    [HttpPut("update")]
    public async Task UpdateToDoList(TodoListUpdateDto todoListUpdateDto)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access forbidden");

        var validator = new TodoListUpdateDtoValidator();
        var result = validator.Validate(todoListUpdateDto);
        if (!result.IsValid)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
            throw new ValidationFailedException(errors);
        }

        var toDoList = this.mapper.Map<TodoList>(todoListUpdateDto);
        toDoList.UserId = long.Parse(userId);

        await this.todoListRepository.UpdateTodoListAsync(toDoList);
    }
}
