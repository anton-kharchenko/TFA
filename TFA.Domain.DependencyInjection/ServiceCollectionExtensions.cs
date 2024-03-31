using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.UseCases.CreateForum;
using TFA.Domain.Interfaces.UseCases.CreateTopic;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Interfaces.UseCases.GetTopics;
using TFA.Domain.Interfaces.UseCases.SignIn;
using TFA.Domain.Interfaces.UseCases.SignOn;
using TFA.Domain.Models;
using TFA.Domain.Resolvers.Forum;
using TFA.Domain.Resolvers.Topic;
using TFA.Domain.UseCases.CreateForum;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopic;
using TFA.Domain.UseCases.SignIn;
using TFA.Domain.UseCases.SignOn;

namespace TFA.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddForumDomain(this IServiceCollection services)
    {
        services
            .AddScoped<ICreateForumUseCase, CreateForumUseCase>()
            .AddScoped<IIntentionResolver, ForumIntentionResolver>()
            .AddScoped<ICreateTopicUseCase, CreateTopicUseCase>()
            .AddScoped<IGetForumsUseCase, GetForumsUseCase>()
            .AddScoped<IGetTopicsUseCase, GetTopicsUseCase>()
            .AddScoped<IIntentionResolver, TopicIntentionResolver>()
            .AddScoped<ISignInUseCase, SignInUseCase>()
            .AddScoped<ISignOnUseCase, SignOnUseCase>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<ISymmetricDecryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<ISymmetricEncryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<IPasswordManager, PasswordManager>();

        services
            .AddScoped<IIntentionManager, IntentionManager>()
            .AddScoped<IIdentityProvider, IdentityProvider>();

        services.AddMemoryCache();

        services.AddValidatorsFromAssemblyContaining<Forum>();
    }
}