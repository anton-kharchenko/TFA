using FluentValidation;
using TFA.Domain.Commands.SignOut;

namespace TFA.Domain.Validations.SignOut;

internal class SignOutCommandValidator : AbstractValidator<SignOutCommand>;