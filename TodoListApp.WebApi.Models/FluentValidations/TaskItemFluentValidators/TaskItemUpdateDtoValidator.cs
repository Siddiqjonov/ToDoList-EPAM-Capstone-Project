#pragma warning disable SA1200 // Using directives should be placed correctly
using FluentValidation;
using TodoListApp.WebApi.Models.Dtos.TaskItemDtos;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Models.FluentValidations.TaskItemFluentValidators;

public class TaskItemUpdateDtoValidator : AbstractValidator<TaskItemUpdateDto>
{
    public TaskItemUpdateDtoValidator()
    {
        _ = this.RuleFor(x => x.TaskItemId)
            .GreaterThan(0).WithMessage("TaskItemId must be greater than 0");

        _ = this.RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title can't be longer than 100 characters");

        _ = this.RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description can't be longer than 500 characters");

        _ = this.RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Due date can't be in the past")
            .When(x => x.DueDate.HasValue);

        _ = this.RuleFor(x => x.TodoListId)
            .GreaterThan(0).WithMessage("TodoListId must be a valid ID");
    }
}
