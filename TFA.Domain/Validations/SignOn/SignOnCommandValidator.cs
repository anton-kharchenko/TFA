using FluentValidation;
using TFA.Domain.Keys;
using TFA.Domain.UseCases.SignOn;

namespace TFA.Domain.Validations.SignOn;

internal class SignOnCommandValidator  : AbstractValidator<SignOnCommand>
{
    public SignOnCommandValidator()
    {
        RuleFor(c => c.Login).Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty);

        RuleFor(c => c.Password)
            .NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty);
    }
}