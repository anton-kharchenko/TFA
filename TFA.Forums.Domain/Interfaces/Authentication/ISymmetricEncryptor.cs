﻿namespace TFA.Forums.Domain.Interfaces.Authentication;

internal interface ISymmetricEncryptor
{
    Task<string> EncryptAsync(string plainText, byte[] key, CancellationToken cancellationToken);
}