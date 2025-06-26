#pragma warning disable SA1200 // Using directives should be placed correctly
using TodoListApp.Core.Errors;
using TodoListApp.WebApi.Helpers;
using TodoListApp.WebApi.Helpers.Security;
using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Models.Dtos.AppUserDtos;
using TodoListApp.WebApi.Models.Dtos.AuthDtos;
using TodoListApp.WebApi.Models.FluentValidations.AuthFluentValidators;
using TodoListApp.WebApi.Models.FluentValidations.UserFluentValidators;
using TodoListApp.WebApi.Models.Models;
#pragma warning disable format

namespace TodoListApp.WebApi.AuthService;

public class AuthServicee : IAuthService
{
    private readonly IUserRepositroy userRepositroy;
    private readonly ITokenService tokenService;
    private readonly IRefreshTokenRepository refreshTokenRepository;
    private readonly IEmailSender emailSender;
    private readonly IUserRepositroy repositroy;

    public AuthServicee(IUserRepositroy userRepositroy, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository, IEmailSender emailSender, IUserRepositroy repositroy)
    {
        this.userRepositroy = userRepositroy;
        this.tokenService = tokenService;
        this.refreshTokenRepository = refreshTokenRepository;
        this.emailSender = emailSender;
        this.repositroy = repositroy;
    }

    public async Task LogOutAsync(string token)
    {
        await this.refreshTokenRepository.RemoveRefreshTokenAsync(token);
    }

    public async Task<string> LoginUserAsync(UserLogInDto userLogInDto)
    {
        var logInValidator = new UserLogInDtoValidator();
        var result = logInValidator.Validate(userLogInDto);

        if (!result.IsValid)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
            throw new ValidationFailedException(errors);
        }

        var user = await this.userRepositroy.SelectUserByUserNameAsync(userLogInDto.UserName);
        _ = user.PasswordHash ?? throw new DatabaseException("The hashed password was not saved to DB plase sign Up and sing In again");

        var checkUserPassword = PasswordHasher.Verify(userLogInDto.Password, user.PasswordHash, user.Salt);
        if (!checkUserPassword)
        {
            throw new UnauthorizedException("User or password incorrect");
        }

        var userGetDto = new AppUserGetDto()
        {
            UserId = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber!,
        };

        var token = this.tokenService.GenerateTokent(userGetDto);

        return token;
    }

    public async Task<ServiceResult> SignUpUserAsync(UserCreateDto userCreateDto)
    {
        var userValidator = new UserCreateDtoValidator();
        var result = userValidator.Validate(userCreateDto);

        if (!result.IsValid)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
            return new ServiceResult(false, errors);
        }

        var existingUser = await this.userRepositroy.SelectUserByEmailAsync(userCreateDto.Email);
        if (existingUser != null)
        {
            return new ServiceResult(false, "Email already registered.");
        }

        return await this.emailSender.SendEmailConfirmationCodeAsync(userCreateDto.Email);
    }

    public async Task<long> FinalizeUserCreationAsync(UserCreateDto userCreateDto)
    {
        var (hash, salt) = PasswordHasher.Hasher(userCreateDto.Password);

        var user = new ApplicationUser
        {
            UserName = userCreateDto.UserName,
            Email = userCreateDto.Email,
            PhoneNumber = userCreateDto.PhoneNumber,
            PasswordHash = hash,
            Salt = salt,
        };

        var userId = await this.userRepositroy.InsertUserAsync(user);

        var insertCount = await this.repositroy.InsertUser(user);
        if (insertCount < 0)
        {
            throw new DatabaseException("User was not added");
        }

        return userId;
    }

    public async Task<bool> ConfirmEmailAsync(VerifyEmailRequest dto)
    {
        var validator = new VerifyEmailRequestValidator();
        var result = validator.Validate(dto);
        if (!result.IsValid)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
            throw new ValidationFailedException(errors);
        }

        return await this.emailSender.VerifyEmailConfirmationAsync(dto.Email, dto.Code);
    }

    public async Task<ServiceResult> RequestPasswordResetAsync(string email)
    {
        return await this.emailSender.RequestPasswordReset(email);
    }

    public async Task<ServiceResult> VerifyPasswordResetAsync(VerifyResetRequest request)
    {
        return await this.emailSender.VerifyPasswordReset(request);
    }
}
