#pragma warning disable SA1200 // Using directives should be placed correctly
using FluentValidation;
using TodoListApp.WebApi.Models.Dtos.ToDoListDtos;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.Models.FluentValidations.ToDoListFluentValidators;

public class TodoListUpdateDtoValidator : AbstractValidator<TodoListUpdateDto>
{
    public TodoListUpdateDtoValidator()
    {
        _ = this.RuleFor(x => x.TodoListId)
            .GreaterThan(0).WithMessage("TodoListId must be greater than 0.");

        _ = this.RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        _ = this.RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        _ = this.RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserId must be greater than 0.");
    }
}
