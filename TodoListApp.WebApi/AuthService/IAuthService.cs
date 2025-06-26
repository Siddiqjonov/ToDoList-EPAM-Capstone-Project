#pragma warning disable SA1200 // Using directives should be placed correctly
using TodoListApp.WebApi.Models.Dtos.AppUserDtos;
using TodoListApp.WebApi.Models.Dtos.AuthDtos;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.AuthService;

public interface IAuthService
{
    Task<bool> ConfirmEmailAsync(VerifyEmailRequest dto);

    Task<ServiceResult> SignUpUserAsync(UserCreateDto userCreateDto);

    Task<string> LoginUserAsync(UserLogInDto userLogInDto);

    Task LogOutAsync(string token);

    Task<long> FinalizeUserCreationAsync(UserCreateDto userCreateDto);

    Task<ServiceResult> RequestPasswordResetAsync(string email);

    Task<ServiceResult> VerifyPasswordResetAsync(VerifyResetRequest request);
}
