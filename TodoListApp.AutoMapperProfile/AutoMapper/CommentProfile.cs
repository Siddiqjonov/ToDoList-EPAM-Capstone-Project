#pragma warning disable SA1200 // Using directives should be placed correctly
using AutoMapper;
using TodoListApp.WebApi.Models.Dtos.CommentDtos;
using TodoListApp.WebApi.Models.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.AutoMapperProfile.AutoMapper;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        _ = this.CreateMap<Comment, CommentGetDto>();
        _ = this.CreateMap<CommentCreateDto, Comment>();
        _ = this.CreateMap<CommentUpdateDto, Comment>();
    }
}
