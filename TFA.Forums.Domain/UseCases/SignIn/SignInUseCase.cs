using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Options;
using TFA.Forums.Domain.Authentication;
using TFA.Forums.Domain.Commands.SignIn;
using TFA.Forums.Domain.Configurations;
using TFA.Forums.Domain.Interfaces.Authentication;
using TFA.Forums.Domain.Interfaces.UseCases.SignIn;
using TFA.Forums.Domain.Keys;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace TFA.Forums.Domain.UseCases.SignIn;

internal class SignInUseCase(
    ISignInStorage storage,
    IPasswordManager passwordManager,
    ISymmetricEncryptor encryptor,
    IOptions<AuthenticationConfiguration> options) : IRequestHandler<SignInCommand, (IIdentity identity, string token)>
{
    public async Task<(IIdentity identity, string token)> Handle(SignInCommand command, CancellationToken cancellationToken)
    {
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