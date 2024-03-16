namespace TFA.Domain.Interfaces.Authentication;

internal interface ISecurityManager
{
    bool ComparePasswords(string password, string salt, string hash);

    (string salt, string hash) GeneratePasswordParts(string password);
}