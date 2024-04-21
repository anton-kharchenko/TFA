namespace TFA.Forum.Domain.Interfaces.Authentication;

internal interface IPasswordManager
{
    (byte[] salt, byte[] hash) GeneratePasswordParts(string password);
    
    bool ComparePasswords(string password, byte[] salt, byte[] hash);

}