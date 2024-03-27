using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;
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

    public SignInUseCaseShould()
    {
        var validator = new Mock<IValidator<SignInCommand>>();

        validator
            .Setup(s => s.ValidateAsync(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var storage = new Mock<ISignInStorage>();
        findUserSetup = storage
            .Setup(s => s.FindUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        passwordManager = new Mock<IPasswordManager>();
        comparePasswordSetup = passwordManager.Setup(m =>
            m.ComparePasswords(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()));

        var encryptor = new Mock<ISymmetricEncryptor>();
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
    public async Task ReturnToken()
    {
        var userId = Guid.Parse("8b7b1b7a-fd96-4ae1-9465-d0d4ed0e20b4");
        findUserSetup.ReturnsAsync(new RecognisedUser
        {
            UserId = userId,
            PasswordHash = [1],
            Salt = [2]
        });
        comparePasswordSetup.Returns(true);
        encryptorSetup.ReturnsAsync("token");

        var (identity, token) = await sut.ExecuteAsync(new SignInCommand("Test", "qwerty"), CancellationToken.None);
        identity.UserId.Should().Be(userId);
        token.Should().Be("token");
    }
}