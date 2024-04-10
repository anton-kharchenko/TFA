using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Authentication;
using TFA.Domain.Commands.SignOut;
using TFA.Domain.Enums;
using TFA.Domain.Exceptions;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.UseCases.SignOut;
using TFA.Domain.UseCases.SignOut;

namespace TFA.Domain.Tests.SignOut;

public class SignOutUseCaseShould
{
    private readonly ISetup<IIdentityProvider, IIdentity> currentIdentitySetup;
    private readonly ISetup<ISignOutStorage, Task> removeSessionSetup;
    private readonly ISetup<IIntentionManager, bool> signOutIsAllowedSetup;
    private readonly Mock<ISignOutStorage> storage;
    private readonly SignOutUseCase sut;

    public SignOutUseCaseShould()
    {
        storage = new Mock<ISignOutStorage>();
        removeSessionSetup = storage.Setup(s => s.RemoveSessionAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        var identityProvider = new Mock<IIdentityProvider>();
        currentIdentitySetup = identityProvider.Setup(p => p.Current);

        var intentionManager = new Mock<IIntentionManager>();
        signOutIsAllowedSetup = intentionManager.Setup(m => m.IsAllowed(It.IsAny<AccountIntentionType>()));

        sut = new SignOutUseCase(intentionManager.Object, identityProvider.Object, storage.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenUserIsNotAuthenticated()
    {
        signOutIsAllowedSetup.Returns(false);

        await sut.Invoking(s => s.Handle(new SignOutCommand(), CancellationToken.None))
            .Should().ThrowAsync<IntentionManagerException>();
    }

    [Fact]
    public async Task RemoveCurrentIdentitySession()
    {
        signOutIsAllowedSetup.Returns(true);
        var sessionId = Guid.Parse("AFBB9DFF-A067-8E11-BA34-A3C32A7B0943");
        currentIdentitySetup.Returns(new User(Guid.Empty, sessionId));
        removeSessionSetup.Returns(Task.CompletedTask);

        await sut.Handle(new SignOutCommand(), new CancellationToken());

        storage.Verify(s => s.RemoveSessionAsync(sessionId, It.IsAny<CancellationToken>()), Times.Once);
        storage.VerifyNoOtherCalls();
    }
}