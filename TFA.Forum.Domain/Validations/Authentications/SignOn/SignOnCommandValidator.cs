using FluentValidation;
using TFA.Forum.Domain.Commands.SignOn;
using TFA.Forum.Domain.Keys;

namespace TFA.Forum.Domain.Validations.Authentications.SignOn;

internal class SignOnCommandValidator  : AbstractValidator<SignOnCommand>
{
    public SignOnCommandValidator()
    {
        RuleFor(c => c.Login)
            .NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty)
            .MaximumLength(20).WithErrorCode(ValidationErrorCodeKeys.TooLong);

        RuleFor(c => c.Password)
            .NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty);
    }
}