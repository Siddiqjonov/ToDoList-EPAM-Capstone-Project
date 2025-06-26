#pragma warning disable SA1200 // Using directives should be placed correctly
using AutoMapper;
using TodoListApp.WebApi.Models.Dtos.AppUserDtos;
using TodoListApp.WebApi.Models.Models;

namespace TodoListApp.AutoMapperProfile.AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        _ = this.CreateMap<ApplicationUser, AppUserGetDto>();
    }
}
