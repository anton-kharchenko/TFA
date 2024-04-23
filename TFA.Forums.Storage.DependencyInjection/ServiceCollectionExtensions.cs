using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TFA.Forums.Domain.Interfaces.Storages;
using TFA.Forums.Domain.Interfaces.Storages.Forum;
using TFA.Forums.Domain.Interfaces.Storages.Topic;
using TFA.Forums.Domain.Interfaces.UseCases.GetForums;
using TFA.Forums.Domain.Interfaces.UseCases.GetTopics;
using TFA.Forums.Domain.Interfaces.UseCases.SignIn;
using TFA.Forums.Domain.Interfaces.UseCases.SignOn;
using TFA.Forums.Domain.Interfaces.UseCases.SignOut;
using TFA.Forums.Storage.Configurations;
using TFA.Forums.Storage.Helpers;
using TFA.Forums.Storage.Interfaces;
using TFA.Forums.Storage.Storages;
using TFA.Forums.Storage.Storages.Authentication;
using TFA.Forums.Storage.Storages.Forum;
using TFA.Forums.Storage.Storages.SignIn;
using TFA.Forums.Storage.Storages.SignOn;
using TFA.Forums.Storage.Storages.SignOut;
using TFA.Forums.Storage.Storages.Topic;

namespace TFA.Forums.Storage.DependencyInjection;

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

        services.AddSingleton<IUnitOfWork>(sp => new UnitOfWork(sp));

        services.AddScoped<IGuidFactory, GuidFactory>();
        services.AddScoped<IMomentProvider, MomentProvider>();

        services.AddDbContextPool<ForumDbContext>(opt => { opt.UseNpgsql(connectionString); });

        services.AddMemoryCache();
        services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));
    }
}