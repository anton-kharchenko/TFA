namespace TFA.Domain.Interfaces.Authentication;

internal interface ISymmetricDecryptor
{
    Task<string> DecryptAsync(string encryptedText, byte[] key, CancellationToken cancellationToken);
}