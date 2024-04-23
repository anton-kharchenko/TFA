using FluentValidation;
using TFA.Forums.Domain.Commands.SignOn;
using TFA.Forums.Domain.Keys;

namespace TFA.Forums.Domain.Validations.Authentications.SignOn;

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