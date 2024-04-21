using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TFA.Forum.Domain.Authentication;
using TFA.Forum.Domain.Authorization;
using TFA.Forum.Domain.Interfaces.Authentication;
using TFA.Forum.Domain.Interfaces.Authorization;
using TFA.Forum.Domain.Monitoring;
using TFA.Forum.Domain.Pipes;
using TFA.Forum.Domain.Resolvers.Forum;
using TFA.Forum.Domain.Resolvers.Topic;

namespace TFA.Forum.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddForumDomain(this IServiceCollection services)
    {
        services.AddMediatR(options => 
            options
                .AddOpenBehavior(typeof(MonitoringPipelineBehaviour<,>))
                .AddOpenBehavior(typeof(ValidationPipelineBehaviour<,>))
                .RegisterServicesFromAssemblyContaining<Models.Forum>());

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

        services.AddValidatorsFromAssemblyContaining<Models.Forum>();

        services.AddSingleton<DomainMetrics>();
    }
}