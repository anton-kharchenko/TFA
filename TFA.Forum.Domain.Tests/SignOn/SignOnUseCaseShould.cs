using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using TFA.Forum.Domain.Commands.SignOn;
using TFA.Forum.Domain.Interfaces.Authentication;
using TFA.Forum.Domain.Interfaces.UseCases.SignOn;
using TFA.Forum.Domain.UseCases.SignOn;

namespace TFA.Forum.Domain.Tests.SignOn;

public class SignOnUseCaseShould
{
    private readonly ISetup<ISignOnStorage, Task<Guid>> createUserSetup;
    private readonly ISetup<IPasswordManager, (byte[] salt, byte[] hash)> generatePasswordPartsSetup;
    private readonly Mock<ISignOnStorage> storage;
    private readonly SignOnUseCase sut;

    public SignOnUseCaseShould()
    {
        var passwordManager = new Mock<IPasswordManager>();

        generatePasswordPartsSetup = passwordManager
            .Setup(m => m.GeneratePasswordParts(It.IsAny<string>()));

        storage = new Mock<ISignOnStorage>();
        createUserSetup = storage.Setup(s => s.CreateUserAsync(It.IsAny<string>(), It.IsAny<byte[]>(),
            It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

        sut = new SignOnUseCase(passwordManager.Object, storage.Object);
    }

    [Fact]
    public async Task CreateUser_WithGeneratePasswordParts()
    {
        var salt = new byte[] { 1 };
        var hash = new byte[] { 2 };
        generatePasswordPartsSetup.Returns((salt, hash));

        await sut.Handle(new SignOnCommand("Test", "qwerty"), CancellationToken.None);

        storage.Verify(s => s.CreateUserAsync("Test", salt, hash, It.IsAny<CancellationToken>()));
        storage.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ReturnIdentityOfNewlyCreatedUser()
    {
        var salt = new byte[] { 1 };
        var hash = new byte[] { 2 };
        generatePasswordPartsSetup.Returns((salt, hash));
        createUserSetup.ReturnsAsync(Guid.Parse("edccd271-24b7-4891-809b-03c7d0d3fe0a"));

        var actual = await sut.Handle(new SignOnCommand("Test", "qwerty"), CancellationToken.None);
        actual.UserId.Should().Be(Guid.Parse("edccd271-24b7-4891-809b-03c7d0d3fe0a"));
    }
}