using FluentValidation;
using MediatR;
using TFA.Domain.Authentication;
using TFA.Domain.Commands.SignOn;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.UseCases.SignOn;

namespace TFA.Domain.UseCases.SignOn;

internal class SignOnUseCase(
    IValidator<SignOnCommand> validator,
    IPasswordManager passwordManager,
    ISignOnStorage signOnStorage) : IRequestHandler<SignOnCommand, IIdentity>
{
    public async Task<IIdentity> Handle(SignOnCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var (salt, hash) = passwordManager.GeneratePasswordParts(command.Password);

        var userId = await signOnStorage.CreateUserAsync(command.Login, salt, hash, cancellationToken);

        return new User(userId, Guid.Empty);
    }
}