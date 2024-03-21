using FluentValidation;
using TFA.Domain.Keys;
using TFA.Domain.UseCases.SignOn;

namespace TFA.Domain.Validations.SignOn;

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