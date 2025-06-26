#pragma warning disable SA1200 // Using directives should be placed correctly
using FluentValidation;
using TodoListApp.WebApi.Models.Dtos.AppUserDtos;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Models.FluentValidations.UserFluentValidators;

public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateDtoValidator()
    {
        _ = this.RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters.")
            .MaximumLength(30).WithMessage("Username must not exceed 30 characters.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores.");

        _ = this.RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .Matches(@"^[a-zA-Z0-9._%+-]+@gmail\.com$").WithMessage("Only Gmail addresses are allowed (e.g., example@gmail.com).");

        _ = this.RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");

        _ = this.RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+998\d{9}$").WithMessage("Phone number must be in the format +998901234567.");
    }
}
