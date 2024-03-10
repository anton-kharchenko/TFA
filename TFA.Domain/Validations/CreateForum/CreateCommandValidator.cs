using FluentValidation;
using TFA.Domain.Commands.CreateForum;
using TFA.Domain.Keys;

namespace TFA.Domain.Validations.CreateForum;

public class CreateCommandValidator : AbstractValidator<CreateForumCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodeKeys.Empty)
            .MaximumLength(100)
            .WithErrorCode(ValidationErrorCodeKeys.TooLong);
    }
}