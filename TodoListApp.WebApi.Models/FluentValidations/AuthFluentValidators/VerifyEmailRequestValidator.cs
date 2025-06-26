#pragma warning disable SA1200 // Using directives should be placed correctly
using FluentValidation;
using TodoListApp.WebApi.Models.Dtos.AuthDtos;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Models.FluentValidations.AuthFluentValidators;

public class VerifyEmailRequestValidator : AbstractValidator<VerifyEmailRequest>
{
    public VerifyEmailRequestValidator()
    {
        _ = this.RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        _ = this.RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Confirmation code is required.")
            .Length(6).WithMessage("Confirmation code must be 6 digits.")
            .Matches(@"^\d{6}$").WithMessage("Code must be numeric.");
    }
}
