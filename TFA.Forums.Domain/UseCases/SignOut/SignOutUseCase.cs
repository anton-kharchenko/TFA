using MediatR;
using TFA.Forums.Domain.Extensions;
using TFA.Forums.Domain.Commands.SignOut;
using TFA.Forums.Domain.Enums;
using TFA.Forums.Domain.Interfaces.Authentication;
using TFA.Forums.Domain.Interfaces.Authorization;
using TFA.Forums.Domain.Interfaces.UseCases.SignOut;

namespace TFA.Forums.Domain.UseCases.SignOut;

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