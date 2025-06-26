#pragma warning disable SA1200 // Using directives should be placed correctly
using FluentValidation;
using TodoListApp.WebApi.Models.Dtos.TaskItemDtos;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Models.FluentValidations.TaskItemFluentValidators;

public class TaskItemCreateDtoValidator : AbstractValidator<TaskItemCreateDto>
{
    public TaskItemCreateDtoValidator()
    {
        _ = this.RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title can't be longer than 100 characters");

        _ = this.RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description can't be longer than 500 characters");

        _ = this.RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Due date can't be in the past")
            .When(x => x.DueDate.HasValue);

        _ = this.RuleFor(x => x.TodoListId)
            .NotEmpty().WithMessage("TodoListId is required")
            .GreaterThan(0).WithMessage("TodoListId must be a valid ID");
    }
}
