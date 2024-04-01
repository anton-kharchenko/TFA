using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TFA.Domain.Interfaces.Storages;
using TFA.Domain.Interfaces.Storages.Forum;
using TFA.Domain.Interfaces.Storages.Topic;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Interfaces.UseCases.GetTopics;
using TFA.Domain.Interfaces.UseCases.SignIn;
using TFA.Domain.Interfaces.UseCases.SignOn;
using TFA.Domain.Interfaces.UseCases.SignOut;
using TFA.Storage.Configurations;
using TFA.Storage.Helpers;
using TFA.Storage.Interfaces;
using TFA.Storage.Storages.Authentication;
using TFA.Storage.Storages.Forum;
using TFA.Storage.Storages.SignIn;
using TFA.Storage.Storages.SignOn;
using TFA.Storage.Storages.SignOut;
using TFA.Storage.Storages.Topic;

namespace TFA.Storage.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddForumStorages(this IServiceCollection services, string? connectionString = default)
    {
        services
            .AddScoped<IAuthenticationStorage, AuthenticationStorage>()
            .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
            .AddScoped<ICreateForumStorage, CreateForumStorage>()
            .AddScoped<IGetForumsStorage, GetForumsStorage>()
            .AddScoped<IGetTopicsStorage, GetTopicStorage>()
            .AddScoped<ISignInStorage, SignInStorage>()
            .AddScoped<ISignOnStorage, SignOnStorage>()
            .AddScoped<ISignOutStorage, SignOutStorage>();

        services.AddScoped<IGuidFactory, GuidFactory>();
        services.AddScoped<IMomentProvider, MomentProvider>();

        services.AddDbContextPool<ForumDbContext>(opt => { opt.UseNpgsql(connectionString); });

        services.AddMemoryCache();
        services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));
    }
}