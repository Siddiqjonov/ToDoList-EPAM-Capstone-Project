#pragma warning disable SA1200 // Using directives should be placed correctly
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Core.Errors;
using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Models.Dtos.TagDtos;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/tag")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly ITagRepository tagRepository;
    private readonly IMapper mapper;

    public TagController(ITagRepository tagRepository, IMapper mapper)
    {
        this.tagRepository = tagRepository;
        this.mapper = mapper;
    }

    [HttpGet("getAllTagsAsync")]
    public async Task<List<TagGetDto>> GetAllTagsAsync()
    {
        var userId = this.User.FindFirst("Id")?.Value
            ?? throw new ForbiddenException("Access denied");

        IEnumerable<Tag> tags = await this.tagRepository.SelectAllTagsForUserAsync(long.Parse(userId));

        var tagDtos = this.mapper.Map<List<TagGetDto>>(tags);
        return tagDtos;
    }

    [HttpGet("getTagsForTask")]
    public async Task<List<TagGetDto>> GetTagsForTask([FromQuery] int taskId)
    {
        var tags = await this.tagRepository.SelectTaskItemTagsAsync(taskId);
        return this.mapper.Map<List<TagGetDto>>(tags);
    }

    [HttpPost("addTagToTask")]
    public async Task<IActionResult> AddTagToTask([FromQuery] int taskId, [FromQuery] int tagId)
    {
        await this.tagRepository.AddTagToTaskAsync(taskId, tagId);
        return this.Ok("Tag added to task.");
    }

    [HttpDelete("removeTagFromTask")]
    public async Task<IActionResult> RemoveTagFromTask([FromQuery] int taskId, [FromQuery] int tagId)
    {
        await this.tagRepository.RemoveTagFromTaskAsync(taskId, tagId);
        return this.Ok("Tag removed from task.");
    }

    [HttpPost("createTag")]
    public async Task<long> CreateTag([FromQuery] string tagName, [FromQuery] int taskId)
    {
        if (string.IsNullOrWhiteSpace(tagName) || tagName.Length > 100)
        {
            throw new InvalidArgumentException("Tag name must not be empty and must be less than 100 characters.");
        }

        var tag = new Tag
        {
            Name = tagName,
        };

        long tagId = await this.tagRepository.CreateTagAndAssignToTaskAsync(tag, taskId);

        return tagId;
    }
}
