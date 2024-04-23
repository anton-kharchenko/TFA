using FluentValidation;
using TFA.Forums.Domain.Commands.CreateTopic;
using TFA.Forums.Domain.Keys;

namespace TFA.Forums.Domain.Validations.Commands.CreateTopic;

internal class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
{
    public CreateTopicCommandValidator()
    {
        RuleFor(c => c.ForumId).NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty);
        RuleFor(c => c.Title).Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty)
            .MaximumLength(100).WithErrorCode(ValidationErrorCodeKeys.TooLong);
    }
}