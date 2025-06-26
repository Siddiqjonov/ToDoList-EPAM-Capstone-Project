#pragma warning disable SA1200 // Using directives should be placed correctly
using AutoMapper;
using TodoListApp.WebApi.Models.Dtos.TagDtos;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.AutoMapperProfile.AutoMapper;
public class TagProfile : Profile
{
    public TagProfile()
    {
        _ = this.CreateMap<Tag, TagGetDto>();
    }
}
