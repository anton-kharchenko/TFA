using FluentValidation;
using MediatR;
using TFA.Forums.Domain.Authentication;
using TFA.Forums.Domain.Commands.SignOn;
using TFA.Forums.Domain.Interfaces.Authentication;
using TFA.Forums.Domain.Interfaces.UseCases.SignOn;

namespace TFA.Forums.Domain.UseCases.SignOn;

internal class SignOnUseCase(
    IPasswordManager passwordManager,
    ISignOnStorage signOnStorage) : 
    IRequestHandler<SignOnCommand, IIdentity>
{
    public async Task<IIdentity> Handle(SignOnCommand command, CancellationToken cancellationToken)
    {
        var (salt, hash) = passwordManager.GeneratePasswordParts(command.Password);

        var userId = await signOnStorage.CreateUserAsync(command.Login, salt, hash, cancellationToken);

        return new User(userId, Guid.Empty);
    }
}