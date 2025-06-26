#pragma warning disable SA1200 // Using directives should be placed correctly
using AutoMapper;
using TodoListApp.WebApi.Models.Dtos.TaskItemDtos;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.AutoMapperProfile.AutoMapper;
public class TaskItemProfile : Profile
{
    public TaskItemProfile()
    {
        _ = this.CreateMap<TaskItem, TaskItemGetDto>()
                .ForMember(dest => dest.TaskItemId, opt => opt.MapFrom(src => src.Id));
        _ = this.CreateMap<TaskItemCreateDto, TaskItem>();
        _ = this.CreateMap<TaskItemUpdateDto, TaskItem>();
    }
}
