using FluentValidation;
using TFA.Forum.Domain.Commands.SignIn;
using TFA.Forum.Domain.Keys;

namespace TFA.Forum.Domain.Validations.Authentications.SignIn;

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