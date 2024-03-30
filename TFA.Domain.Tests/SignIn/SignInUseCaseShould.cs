using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Commands.SignIn;
using TFA.Domain.Configurations;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.UseCases.SignIn;
using TFA.Domain.Share;
using TFA.Domain.UseCases.SignIn;

namespace TFA.Domain.Tests.SignIn;

public class SignInUseCaseShould
{
    private readonly SignInUseCase sut;
    private readonly ISetup<ISignInStorage, Task<RecognisedUser?>> findUserSetup;
    private readonly Mock<IPasswordManager> passwordManager;
    private readonly ISetup<IPasswordManager, bool> comparePasswordSetup;
    private readonly ISetup<ISymmetricEncryptor, Task<string>> encryptorSetup;
    private readonly ISetup<ISignInStorage, Task<Guid>> createSessionSetup;
    private readonly Mock<ISignInStorage> storage;
    private readonly Mock<ISymmetricEncryptor> encryptor;

    public SignInUseCaseShould()
    {
        var validator = new Mock<IValidator<SignInCommand>>();

        validator
            .Setup(s => s.ValidateAsync(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        storage = new Mock<ISignInStorage>();
        findUserSetup = storage
            .Setup(s => s.FindUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));
         createSessionSetup = storage.Setup(s => s.CreateSessionAsync(It.IsAny<Guid>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()));


        passwordManager = new Mock<IPasswordManager>();
        comparePasswordSetup = passwordManager.Setup(m =>
            m.ComparePasswords(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()));

        encryptor = new Mock<ISymmetricEncryptor>();
        encryptorSetup = encryptor
            .Setup(e => e.EncryptAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

        var configuration = new Mock<IOptions<AuthenticationConfiguration>>();
        configuration
            .Setup(s => s.Value)
            .Returns(new AuthenticationConfiguration()
            {
                Base64Key = "4pt2wIcJ08YhAjS/LN+f+g=="
            });
        
        sut = new SignInUseCase(
            validator.Object,
            storage.Object,
            passwordManager.Object,
            encryptor.Object,
            configuration.Object);
    }

    [Fact]
    public async Task ThrowValidationException_WhenUserNotFound()
    {
        findUserSetup.ReturnsAsync(() => null);

        (await sut.Invoking(s => s.ExecuteAsync(new SignInCommand("Test", "qwerty"), CancellationToken.None))
                .Should().ThrowAsync<ValidationException>())
            .Which.Errors.Should().Contain(e => e.PropertyName == "Login");
    }

    [Fact]
    public async Task ThrowValidationException_WhenPasswordDoesntMatch()
    {
        findUserSetup.ReturnsAsync(new RecognisedUser());
        comparePasswordSetup.Returns(false);

        (await sut.Invoking(s => s.ExecuteAsync(new SignInCommand("Test", "qwerty"), CancellationToken.None))
                .Should().ThrowAsync<ValidationException>())
            .Which.Errors.Should().Contain(e => e.PropertyName == "Password");
    }

    [Fact]
    public async Task ReturnTokenAndIdentity()
    {
        var userId = Guid.Parse("8b7b1b7a-fd96-4ae1-9465-d0d4ed0e20b4");
        var sessionId = Guid.Parse("21730616-58CB-888D-A91C-E1D1B0CF33B3");
        
        findUserSetup.ReturnsAsync(new RecognisedUser
        {
            UserId = userId,
            PasswordHash = [1],
            Salt = [2]
        });
        
        comparePasswordSetup.Returns(true);
        createSessionSetup.ReturnsAsync(sessionId);
        encryptorSetup.ReturnsAsync("token");

        var (identity, token) = await sut.ExecuteAsync(new SignInCommand("Test", "qwerty"), CancellationToken.None);
        identity.UserId.Should().Be(userId);
                
        identity.UserId.Should().Be(userId);
        identity.SessionId.Should().Be(sessionId);
        
        token.Should().Be("token");
    }

    [Fact]
    public async Task CreateSession_WhenPasswordMatches()
    {
        var userId = Guid.Parse("AEBD5586-5BB7-4F5B-B6A6-56D5F63351BA");
        var sessionId = Guid.Parse("3B05C77F-541F-45A5-B680-147800895222");
        
        findUserSetup.ReturnsAsync(new RecognisedUser { UserId = userId });
        comparePasswordSetup.Returns(true);
        createSessionSetup.ReturnsAsync(sessionId);

        await sut.ExecuteAsync(new SignInCommand("Test", "qwerty"), CancellationToken.None);
        
        storage.Verify(s=>s.CreateSessionAsync(userId, It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task EncryptSessionIdIntoToken()
    {
        var userId = Guid.Parse("6545950E-066A-8C82-92C8-0DAA372A6B74");
        var sessionId = Guid.Parse("ba91e384-0506-89c9-b298-81a74e91820b");
        
        findUserSetup.ReturnsAsync(new RecognisedUser { UserId = userId });
        comparePasswordSetup.Returns(true);
        createSessionSetup.ReturnsAsync(sessionId);

        await sut.ExecuteAsync(new SignInCommand("Test", "qwerty"), CancellationToken.None);
        encryptor.Verify(s=>s.EncryptAsync("ba91e384-0506-89c9-b298-81a74e91820b", It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));
    }
}