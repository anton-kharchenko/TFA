using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Authentication;
using TFA.Domain.Configurations;
using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Tests.Authentication;

public class AuthenticationServiceShould
{
    private readonly AuthenticationService _sut;
    private readonly ISetup<IAuthenticationStorage, Task<RecognisedUser?>> _findUserSetup;

    public AuthenticationServiceShould()
    {
        var storage = new Mock<IAuthenticationStorage>();
        _findUserSetup = storage.Setup(s => s.FindUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        var securityManager = new Mock<SecurityManager>();
        securityManager.Setup(s => s.ComparePasswords(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        var options = new Mock<IOptions<AuthenticationConfiguration>>();
        
        options.Setup(o => o.Value)
            .Returns(new AuthenticationConfiguration
            {
                Key = "063C7F70-0676-80D5-8277-C989422D76F9",
                Iv = "MoFJcytLCxomHz1anSTc"
            });
            
        var tripleDES = new Mock<Lazy<TripleDES>>();

        _sut = new AuthenticationService(storage.Object, securityManager.Object, tripleDES.Object, options.Object);
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

        var (success, authToken) =
            await _sut.SignInAsync(new BasicSignInCredentials("User", "Password"), CancellationToken.None);
        success.Should().BeTrue();
        authToken.Should().NotBeEmpty();
    }
}