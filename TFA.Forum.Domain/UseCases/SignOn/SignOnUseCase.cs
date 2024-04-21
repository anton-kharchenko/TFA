using FluentValidation;
using MediatR;
using TFA.Forum.Domain.Authentication;
using TFA.Forum.Domain.Commands.SignOn;
using TFA.Forum.Domain.Interfaces.Authentication;
using TFA.Forum.Domain.Interfaces.UseCases.SignOn;

namespace TFA.Forum.Domain.UseCases.SignOn;

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