#pragma warning disable SA1200 // Using directives should be placed correctly
using FluentValidation;
using TodoListApp.WebApi.Models.Dtos.AuthDtos;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Models.FluentValidations.UserFluentValidators;

public class UserLogInDtoValidator : AbstractValidator<UserLogInDto>
{
    public UserLogInDtoValidator()
    {
        _ = this.RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User name is required")
            .MaximumLength(50).WithMessage("User name must be 50 characters or less");

        _ = this.RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 6 characters long")
            .MaximumLength(100).WithMessage("Password must be 100 characters or less");
    }
}
