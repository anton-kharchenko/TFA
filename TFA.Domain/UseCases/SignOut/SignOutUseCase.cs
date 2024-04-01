using TFA.Domain.Commands.SignOut;
using TFA.Domain.Enums;
using TFA.Domain.Extensions;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.UseCases.SignOut;

namespace TFA.Domain.UseCases.SignOut;

internal class SignOutUseCase(
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    ISignOutStorage signOutStorage
) : ISignOutUseCase
{
    public async Task ExecuteAsync(SignOutCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(AccountIntentionType.SignOut);
        var sessionId = identityProvider.Current.SessionId;
        await signOutStorage.RemoveSessionAsync(sessionId, cancellationToken);
    }
}