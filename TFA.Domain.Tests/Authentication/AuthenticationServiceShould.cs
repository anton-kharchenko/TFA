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
    private readonly AuthenticationService sut;
    private readonly ISetup<ISymmetricDecryptor, Task<string>> _setupDecryptor;

    public AuthenticationServiceShould()
    {
        var decryptor = new Mock<ISymmetricDecryptor>();
        _setupDecryptor = decryptor.Setup(d => d.DecryptAsync(
            It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

        var options = new Mock<IOptions<AuthenticationConfiguration>>();
        options.Setup(o => o.Value)
            .Returns(new AuthenticationConfiguration()
            {
                Base64Key = "PPdvcYZNzT/xp+pQrRSmUf/JyC2uN9xx6zZtz2LuZKc="
            });
        
        sut = new AuthenticationService(decryptor.Object, options.Object);
    }

    [Fact]
    public async Task ExtractUserIdentityFromToken()
    {
        _setupDecryptor.ReturnsAsync("e12a40db-9da8-4edf-8868-adc9a084df91");
        
        var actual = await sut.AuthenticateAsync("some token", CancellationToken.None);

        actual.Should().BeEquivalentTo(new User(Guid.Parse("e12a40db-9da8-4edf-8868-adc9a084df91"), Guid.Empty));
    }
}