#pragma warning disable SA1200 // Using directives should be placed correctly
using FluentValidation;
using TodoListApp.WebApi.Models.Dtos.CommentDtos;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Models.FluentValidations.CommentFluentValidators;

public class CommentUpdateDtoValidator : AbstractValidator<CommentUpdateDto>
{
    public CommentUpdateDtoValidator()
    {
        _ = this.RuleFor(x => x.CommentId)
            .GreaterThan(0).WithMessage("CommentId must be greater than 0");

        _ = this.RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(1000).WithMessage("Content must be less than 1000 characters.");
    }
}
