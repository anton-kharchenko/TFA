using MediatR;
using TFA.Forum.Domain.Extensions;
using TFA.Forum.Domain.Commands.SignOut;
using TFA.Forum.Domain.Enums;
using TFA.Forum.Domain.Interfaces.Authentication;
using TFA.Forum.Domain.Interfaces.Authorization;
using TFA.Forum.Domain.Interfaces.UseCases.SignOut;

namespace TFA.Forum.Domain.UseCases.SignOut;

internal class SignOutUseCase(
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    ISignOutStorage signOutStorage
) : IRequestHandler<SignOutCommand>
{
    public async Task Handle(SignOutCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(AccountIntentionType.SignOut);
        var sessionId = identityProvider.Current.SessionId;
        await signOutStorage.RemoveSessionAsync(sessionId, cancellationToken);
    }
}