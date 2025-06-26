#pragma warning disable SA1200 // Using directives should be placed correctly
using TodoListApp.WebApi.Models.Dtos.AuthDtos;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.AuthService;

public interface IEmailSender
{
    Task<ServiceResult> RequestPasswordReset(string email);

    Task<ServiceResult> VerifyPasswordReset(VerifyResetRequest request);

    Task<ServiceResult> SendEmailConfirmationCodeAsync(string email);

    Task<bool> VerifyEmailConfirmationAsync(string email, string code);

    Task<bool> IsEmailConfirmedAsync(string email);
}
