using System.Security.Cryptography;
using FluentAssertions;
using TFA.Domain.Authentication;

namespace TFA.Domain.Tests.Authentication;

public class AesSymmetricEncryptorDecryptorShould()
{
    private readonly AesSymmetricEncryptorDecryptor _sut = new();

    [Fact]
    public async Task ReturnMeaningfulEncryptedString()
    {
        var actual = await _sut.EncryptAsync("Hello, world!", RandomNumberGenerator.GetBytes(32), CancellationToken.None);

        actual.Should().NotBeEmpty();
    }

    [Fact]
    public async Task DecryptEncryptedString_WhenKeyIsSame()
    {
        const string HelloWorld = "Hello, world!";
        var key = RandomNumberGenerator.GetBytes(32);
        var encrypt = await _sut.EncryptAsync(HelloWorld, key, CancellationToken.None);
        var decrypt = await _sut.DecryptAsync(encrypt, key, CancellationToken.None);
        
        decrypt.Should().Be(HelloWorld);
    }

    [Fact]
    public async Task ThrowException_WhenDecryptionWithDifferentKey()
    {
        var key = RandomNumberGenerator.GetBytes(32);
        var encrypted = await _sut.EncryptAsync("Hello, world!", key, CancellationToken.None);
        await _sut.Invoking(s => s.DecryptAsync(encrypted, key, CancellationToken.None))
            .Should().ThrowAsync<CryptographicException>();
    }
}