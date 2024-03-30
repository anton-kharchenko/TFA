using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using TFA.Domain.Authentication;
using TFA.Domain.Commands.SignIn;
using TFA.Domain.Configurations;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.UseCases.SignIn;
using TFA.Domain.Keys;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace TFA.Domain.UseCases.SignIn;

internal class SignInUseCase(
    IValidator<SignInCommand> validator,
    ISignInStorage storage,
    IPasswordManager passwordManager,
    ISymmetricEncryptor encryptor,
    IOptions<AuthenticationConfiguration> options) : ISignInUseCase
{
    public async Task<(IIdentity identity, string token)> ExecuteAsync(SignInCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var recognisedUser = await storage.FindUserAsync(command.Login, cancellationToken);

        if (recognisedUser is null)
        {
            throw new FluentValidation.ValidationException(new[]
            {
                new ValidationFailure
                {
                    PropertyName = nameof(command.Login),
                    ErrorCode = ValidationErrorCodeKeys.Invalid,
                    AttemptedValue = command.Login
                }
            });
        }

        var passwordIsMath = passwordManager.ComparePasswords(command.Password, recognisedUser.Salt, recognisedUser.PasswordHash);

        if (passwordIsMath is false)
        {
            throw new FluentValidation.ValidationException(new[]
            {
                new ValidationFailure
                {
                    PropertyName = nameof(command.Password),
                    ErrorCode = ValidationErrorCodeKeys.Invalid,
                    AttemptedValue = command.Password
                }
            });
        }

        var sessionId = await storage.CreateSessionAsync(recognisedUser.UserId, DateTimeOffset.Now + TimeSpan.FromHours(1), CancellationToken.None);
        
        var token = await encryptor.EncryptAsync(sessionId.ToString(), options.Value.Key, cancellationToken);

        return (new User(recognisedUser.UserId, sessionId), token);
    }
}