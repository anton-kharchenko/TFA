using Moq;
using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq.Language.Flow;
using TFA.Domain.Authentication;
using TFA.Domain.Configurations;
using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Tests.Authentication;

public class AuthenticationServiceShould
{
    private readonly AuthenticationService _sut;
    private readonly ISetup<IAuthenticationStorage, Task<RecognisedUser?>> _findUserSetup;
    private readonly Mock<IAuthenticationStorage> _storage;
    private readonly Mock<IOptions<AuthenticationConfiguration>> _options;

    public AuthenticationServiceShould()
    {
        _storage = new Mock<IAuthenticationStorage>();
        _findUserSetup = _storage.Setup(s => s.FindUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        var securityManager = new Mock<PasswordManager>();
        securityManager.Setup(s => s.ComparePasswords(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        _options = new Mock<IOptions<AuthenticationConfiguration>>();
        
        _options.Setup(o => o.Value)
            .Returns(new AuthenticationConfiguration
            {
                Key = "063C7F70-0676-80D5-8277-C989422D76F9",
                Iv = "MoFJcytLCxomHz1anSTc"
            });
            
        var tripleDES = new Mock<Lazy<TripleDES>>();

        _sut = new AuthenticationService(_storage.Object, securityManager.Object, tripleDES.Object, _options.Object);
    }

    [Fact]
    public async Task ReturnSuccess_WhenUserFound()
    {
        _findUserSetup.ReturnsAsync(new RecognisedUser
        {
            UserId = Guid.Parse("E092576B-537A-8A55-996F-568145256EE3"),
            Salt = "LODOyU5PLnXTQOg3g5_S",
            PasswordHash = "bb824d2aaa8687011e4f449c4c35b768ee6154412aa1fb0199a1ff08c1712c57"
        });

        var (success, authToken) = await _sut.SignInAsync(new BasicSignInCredentials("User", "Password"), CancellationToken.None);
        
        success.Should().BeTrue();
        authToken.Should().NotBeEmpty();
    }

    [Fact]
    public async Task AuthenticateUser_AfterSignIn()
    {
        var userId = Guid.Parse("5C982883-995B-84DC-BCDE-69F17174C98C");
        _findUserSetup.ReturnsAsync(new RecognisedUser { UserId =  userId });

        var (success, authToken) = await _sut.SignInAsync(new BasicSignInCredentials("User", "Password"), CancellationToken.None);

        var identity = await _sut.AuthenticateAsync(authToken, CancellationToken.None);
        identity.Should().Be(userId);
    }

    [Fact]
    public async Task SighInUser_WhenPasswordMatch()
    {
        var password = "Nushiba";

        var securityManager = new PasswordManager();
        var (salt, hash) = securityManager.GeneratePasswordParts(password);

        _findUserSetup.ReturnsAsync(new RecognisedUser()
        {
            UserId = Guid.Parse("86A3EFEB-FF57-878F-A41E-AFD2C1E5E352"),
            Salt = salt,
            PasswordHash = hash
        });

        var authenticationService = new AuthenticationService(_storage.Object, securityManager, new Lazy<TripleDES>() , _options.Object);
        
        var localSut = authenticationService;
        var (success, authToken) = await localSut.SignInAsync(new BasicSignInCredentials("User", password), CancellationToken.None);
        success.Should().BeTrue();
    }
}