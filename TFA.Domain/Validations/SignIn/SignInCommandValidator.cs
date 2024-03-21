using FluentValidation;
using TFA.Domain.Keys;
using TFA.Domain.UseCases.SignIn;

namespace TFA.Domain.Validations.SignIn;

internal class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(c => c.Login)
            .NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty)
            .MaximumLength(20).WithErrorCode(ValidationErrorCodeKeys.TooLong);

        RuleFor(c => c.Password)
            .NotEmpty().WithErrorCode(ValidationErrorCodeKeys.Empty);
    }
}