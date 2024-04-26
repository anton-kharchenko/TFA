using FluentValidation;
using TFA.Forums.Domain.Commands.CreateComment;
using TFA.Forums.Domain.Keys;

namespace TFA.Forums.Domain.Validations.Commands.CreateComment;

internal class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(c => c.TopicId)
            .NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty);
        RuleFor(c => c.Text)
            .NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty);
    }
}