#pragma warning disable SA1200 // Using directives should be placed correctly
using AutoMapper;
using TodoListApp.WebApi.Models.Dtos.ToDoListDtos;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.AutoMapperProfile.AutoMapper;

public class ToDoListProfile : Profile
{
    public ToDoListProfile()
    {
        _ = this.CreateMap<TodoList, ToDoListGetDto>();
        _ = this.CreateMap<TodoListCreateDto, TodoList>();
        _ = this.CreateMap<TodoListUpdateDto, TodoList>();
    }
}
