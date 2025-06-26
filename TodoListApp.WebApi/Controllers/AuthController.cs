#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.AuthService;
using TodoListApp.WebApi.Models.Dtos.AppUserDtos;
using TodoListApp.WebApi.Models.Dtos.AuthDtos;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Controllers;
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;
    private readonly ILogger<AuthController> logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
        this.logger = logger;
    }

    [HttpPost("signIn")]
    public async Task<string> SignIn([FromBody] UserLogInDto user)
    {
        var result = await this.authService.LoginUserAsync(user);
        return result;
    }

    [HttpDelete("logOut")]
    public async Task LogOut([FromQuery] string token)
    {
        this.logger.LogInformation("user longed out");
        await this.authService.LogOutAsync(token);
    }

    [HttpPost("signUp")]
    public async Task<long> SignUp([FromBody] UserCreateDto user)
    {
        this.logger.LogInformation("User signed up");
        return await this.authService.FinalizeUserCreationAsync(user);
    }

    [HttpPost("request-password-reset")]
    public async Task<ServiceResult> RequestPasswordReset([FromQuery] string email)
    {
        this.logger.LogInformation("Reset password api is called");
        return await this.authService.RequestPasswordResetAsync(email);
    }

    [HttpPost("verify-password-reset")]
    public async Task<ServiceResult> VerifyPasswordReset([FromBody] VerifyResetRequest request)
    {
        this.logger.LogInformation("Verify password requet sent");
        return await this.authService.VerifyPasswordResetAsync(request);
    }
}
