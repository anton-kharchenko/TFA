using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Models;
using TFA.Domain.Monitoring;
using TFA.Domain.Resolvers.Forum;
using TFA.Domain.Resolvers.Topic;

namespace TFA.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddForumDomain(this IServiceCollection services)
    {
        services.AddMediatR(options => 
            options.RegisterServicesFromAssemblyContaining<Forum>());

        services
            .AddScoped<IIntentionManager, IntentionManager>()
            .AddScoped<IIdentityProvider, IdentityProvider>()
            .AddScoped<IIntentionResolver, ForumIntentionResolver>()
            .AddScoped<IIntentionResolver, TopicIntentionResolver>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<ISymmetricDecryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<ISymmetricEncryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<IPasswordManager, PasswordManager>();

        services.AddMemoryCache();

        services.AddValidatorsFromAssemblyContaining<Forum>();

        services.AddSingleton<DomainMetrics>();
    }
}