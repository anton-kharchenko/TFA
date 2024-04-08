using FluentValidation;
using TFA.Domain.Commands.CreateForum;
using TFA.Domain.Keys;

namespace TFA.Domain.Validations.Commands.CreateForum;

public class CreateForumCommandValidator : AbstractValidator<CreateForumCommand>
{
    public CreateForumCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodeKeys.Empty)
            .MaximumLength(100)
            .WithErrorCode(ValidationErrorCodeKeys.TooLong);
    }
}