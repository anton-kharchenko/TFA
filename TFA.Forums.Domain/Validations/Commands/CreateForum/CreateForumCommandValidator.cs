using FluentValidation;
using TFA.Forums.Domain.Commands.CreateForum;
using TFA.Forums.Domain.Keys;

namespace TFA.Forums.Domain.Validations.Commands.CreateForum;

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