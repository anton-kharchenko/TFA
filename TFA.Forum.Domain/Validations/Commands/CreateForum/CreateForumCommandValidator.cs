using FluentValidation;
using TFA.Forum.Domain.Commands.CreateForum;
using TFA.Forum.Domain.Keys;

namespace TFA.Forum.Domain.Validations.Commands.CreateForum;

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