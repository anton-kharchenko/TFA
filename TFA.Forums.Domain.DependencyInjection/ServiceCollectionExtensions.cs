using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TFA.Forums.Domain.Authentication;
using TFA.Forums.Domain.Authorization;
using TFA.Forums.Domain.Interfaces.Authentication;
using TFA.Forums.Domain.Interfaces.Authorization;
using TFA.Forums.Domain.Monitoring;
using TFA.Forums.Domain.Pipes;
using TFA.Forums.Domain.Resolvers;

namespace TFA.Forums.Domain.DependencyInjection;

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