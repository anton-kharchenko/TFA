using FluentValidation;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.UseCases.SignIn;

namespace TFA.Domain.UseCases.SignIn;

internal class SignInUseCase(
    IValidator<SignInCommand> validator,
    ISignInStorage storage,
    IPasswordManager passwordManager,
    ISymmetricEncryptor encryptor) : ISignInUseCase
{
    public async Task<(IIdentity identity, string token)> ExecuteAsync(SignInCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var recognisedUser = await storage.FindUserAsync(command.Login, cancellationToken);

        if (recognisedUser is null)
        {
            throw new Exception();
        }

        var passwordIsMath = passwordManager.ComparePasswords(command.Password, recognisedUser.Salt, recognisedUser.PasswordHash);

        if (passwordIsMath is false)
        {
            throw new Exception();
        }

        encryptor.EncryptAsync(recognisedUser.UserId.ToString(), );
    }
}