#pragma warning disable SA1200 // Using directives should be placed correctly
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Core.Errors;
using TodoListApp.WebApi.Enums;
using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Models.Dtos.TaskItemDtos;
using TodoListApp.WebApi.Models.FluentValidations.TaskItemFluentValidators;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/taskItem")]
[ApiController]
public class TaskItemsController : ControllerBase
{
    private readonly ITaskItemRepository taskItemRepository;
    private readonly IMapper mapper;

    public TaskItemsController(ITaskItemRepository taskItemRepository, IMapper mapper)
    {
        this.taskItemRepository = taskItemRepository;
        this.mapper = mapper;
    }

    [HttpGet("getTasksByToDoListId/{todoListId}")]
    public async Task<List<TaskItemGetDto>> GetTasksByToDoListId(long todoListId)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access forbidden");

        List<TaskItem> taskItems = await this.taskItemRepository.SelectTasksByTodoListIdAsync(long.Parse(userId), todoListId)
            ?? throw new EntityNotFoundException($"Tasks with userId {userId} not found");

        var taskItemDtos = this.mapper.Map<List<TaskItemGetDto>>(taskItems);
        for (int i = 0; i < taskItemDtos.Count; i++)
        {
            taskItemDtos[i].TaskItemId = taskItems[i].Id;
            taskItemDtos[i].CreatedDate = taskItems[i].CreatedDate;
        }

        return taskItemDtos;
    }

    [HttpGet("getTaskById/{taskId}")]
    public async Task<TaskItemGetDto> GetTaskById(long taskId)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access forbidden");

        var taskItem = await this.taskItemRepository.SelectTaskByIdAsync(taskId, long.Parse(userId));

        var taskItemDto = this.mapper.Map<TaskItemGetDto>(taskItem);
        return taskItemDto;
    }

    [HttpPost("addTask")]
    public async Task<long> CreateTask(TaskItemCreateDto taskItemDto)
    {
        var validator = new TaskItemCreateDtoValidator();
        var result = validator.Validate(taskItemDto);
        if (!result.IsValid)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
            throw new ValidationFailedException(errors);
        }

        var taskItem = this.mapper.Map<TaskItem>(taskItemDto);
        taskItem.IsCompleted = false;

        var taskItemId = await this.taskItemRepository.InsertTaskAsync(taskItem);
        return taskItemId;
    }

    [HttpDelete("deleteTask/{taskId}")]
    public async Task DeleteTask(long taskId)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access forbidden");

        if (taskId <= 0)
        {
            throw new InvalidArgumentException("Task Id should be greater than 0");
        }

        await this.taskItemRepository.DeleteTaskAsync(taskId, long.Parse(userId));
    }

    [HttpPut("updateTask")]
    public async Task UpdateTask(TaskItemUpdateDto taskItemUpdateDto)
    {
        var validator = new TaskItemUpdateDtoValidator();
        var result = validator.Validate(taskItemUpdateDto);
        if (!result.IsValid)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
            throw new ValidationFailedException(errors);
        }

        TaskItem taskItem = this.mapper.Map<TaskItem>(taskItemUpdateDto);

        taskItem.Id = taskItemUpdateDto.TaskItemId;

        await this.taskItemRepository.UpdateTaskAsync(taskItem);
    }

    [HttpGet("getOverDueTasks")]
    public async Task<List<TaskItemGetDto>> GetOverDueItems()
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access forbidden");

        var taskItems = await this.taskItemRepository.SelectOverDueTasksAsync(long.Parse(userId));

        var taskItemDtos = this.mapper.Map<List<TaskItemGetDto>>(taskItems);

        return taskItemDtos;
    }

    [HttpGet("getAllTasksAssignedToUser")]
    public async Task<List<TaskItemGetDto>> GetAssignedTasks()
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access forbidden");

        var taskItems = await this.taskItemRepository.SelectTasksAssignedToUserAsync(long.Parse(userId));
        var taskImteDtos = this.mapper.Map<List<TaskItemGetDto>>(taskItems);
        return taskImteDtos;
    }

    [HttpGet("filterTasksAssignedToUser")]
    public async Task<List<TaskItemGetDto>> FilterTasksAssignedToUser([FromQuery] bool? isCompleted = false)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access forbidden");

        var taskItems = await this.taskItemRepository.SelectTasksAssignedToUserAsync(long.Parse(userId), isCompleted);

        var taskImteDtos = this.mapper.Map<List<TaskItemGetDto>>(taskItems);

        return taskImteDtos;
    }

    [HttpGet("getSortedAssignedTasksByDuedataOrTitle")]
    public async Task<List<TaskItemGetDto>> GetSortedAssignedTasks(
        [FromQuery] bool? isCompleted = null,
        [FromQuery] TaskSortBy sortBy = TaskSortBy.DueDate,
        [FromQuery] bool ascending = true)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access forbidden");

        var taskItems = await this.taskItemRepository.SortAssignedTasksByTitleOrDueDateAsync(long.Parse(userId), isCompleted, sortBy, ascending);

        var taskImteDtos = this.mapper.Map<List<TaskItemGetDto>>(taskItems);

        return taskImteDtos;
    }

    [HttpPatch("updateTaskStatus/{taskId}")]
    public async Task UpdateTaskStatus(long taskId, [FromQuery] bool isCompleted)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access forbidden");

        await this.taskItemRepository.UpdateTaskStatusAsync(taskId, long.Parse(userId), isCompleted);
    }

    [HttpGet("searchTasks")]
    public async Task<List<TaskItemGetDto>> SearchTasks(
    [FromQuery] string? title,
    [FromQuery] DateTime? createdDate,
    [FromQuery] DateTime? dueDate)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access forbidden");

        var taskItems = await this.taskItemRepository.SearchTasksAsync(long.Parse(userId), title, createdDate, dueDate);

        var taskImteDtos = this.mapper.Map<List<TaskItemGetDto>>(taskItems);

        return taskImteDtos;
    }

    [HttpGet("getTaskDetails/{taskId}")]
    public async Task<TaskItemGetDto> GetTaskDetails(long taskId)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access denied");

        var taskItem = await this.taskItemRepository.SelectTaskDetailsAsync(taskId, long.Parse(userId));

        var taskItemDto = this.mapper.Map<TaskItemGetDto>(taskItem);
        return taskItemDto;
    }

    [HttpGet("getTasksByTagId/{tagId}")]
    public async Task<List<TaskItemGetDto>> GetTasksByTagId(long tagId)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access denied");

        if (tagId <= 0)
        {
            throw new InvalidArgumentException("TagId must be positive and greater than 0");
        }

        var taskItems = await this.taskItemRepository.SelectTasksByTagIdAsync(long.Parse(userId), tagId);

        var taskItemDtos = this.mapper.Map<List<TaskItemGetDto>>(taskItems);
        return taskItemDtos;
    }

    [HttpPost("addTagToTask/{tagId}/{taskId}")]
    public async Task<bool> AddTagToTask(long tagId, long taskId)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access denied");

        if (tagId <= 0 && taskId <= 0)
        {
            throw new InvalidArgumentException("TaskId and tagId must be positive and greater than 0");
        }

        var result = await this.taskItemRepository.AddTagToTaskAsync(long.Parse(userId), taskId, tagId);
        return result;
    }

    [HttpDelete("removeTagFromTask/{taskId}/{tagId}")]
    public async Task RemoveTagFromTask(long taskId, long tagId)
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access denied");

        if (taskId <= 0 || tagId <= 0)
        {
            throw new InvalidArgumentException("TaskId and TagId must be positive and greater than 0");
        }

        await this.taskItemRepository.RemoveTagFromTaskAsync(long.Parse(userId), taskId, tagId);
    }
}
