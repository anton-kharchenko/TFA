using FluentAssertions;
using TFA.Forum.Domain.Authentication;

namespace TFA.Forum.Domain.Tests.Authentication;

public class PasswordManagerShould
{
    private static readonly byte[] EmptySalt = Enumerable.Repeat((byte)0, 100).ToArray();
    private static readonly byte[] EmptyHash = Enumerable.Repeat((byte)0, 32).ToArray();
    private readonly PasswordManager sut = new();

    [Theory]
    [InlineData("password")]
    [InlineData("qwerty123")]
    public void GenerateMeaningfulSaltAndHash(string password)
    {
        var (salt, hash) = sut.GeneratePasswordParts(password);

        salt.Should().HaveCount(100).And.NotBeEquivalentTo(EmptySalt);
        hash.Should().HaveCount(32).And.NotBeEquivalentTo(EmptyHash);
    }

    [Fact]
    public void ReturnTrue_WhenPasswordMatch()
    {
        const string Password = "qwerty123";
        var (salt, hash) = sut.GeneratePasswordParts(Password);
        sut.ComparePasswords(Password, salt, hash).Should().BeTrue();
    }

    [Fact]
    public void ReturnTrue_WhenPasswordNotMatch()
    {
        const string Password = "qwerty";
        var (salt, hash) = sut.GeneratePasswordParts(Password);
        sut.ComparePasswords("different_password", salt, hash).Should().BeFalse();
    }
}