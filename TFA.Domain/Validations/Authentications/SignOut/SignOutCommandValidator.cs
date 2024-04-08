using FluentValidation;
using TFA.Domain.Commands.SignOut;

namespace TFA.Domain.Validations.Authentications.SignOut;

internal class SignOutCommandValidator : AbstractValidator<SignOutCommand>;