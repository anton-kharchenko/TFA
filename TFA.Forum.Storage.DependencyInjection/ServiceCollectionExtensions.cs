using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TFA.Forum.Domain.Interfaces.Storages;
using TFA.Forum.Domain.Interfaces.Storages.Forum;
using TFA.Forum.Domain.Interfaces.Storages.Topic;
using TFA.Forum.Domain.Interfaces.UseCases.GetForums;
using TFA.Forum.Domain.Interfaces.UseCases.GetTopics;
using TFA.Forum.Domain.Interfaces.UseCases.SignIn;
using TFA.Forum.Domain.Interfaces.UseCases.SignOn;
using TFA.Forum.Domain.Interfaces.UseCases.SignOut;
using TFA.Forum.Storage.Configurations;
using TFA.Forum.Storage.Helpers;
using TFA.Forum.Storage.Interfaces;
using TFA.Forum.Storage.Storages;
using TFA.Forum.Storage.Storages.Authentication;
using TFA.Forum.Storage.Storages.Forum;
using TFA.Forum.Storage.Storages.SignIn;
using TFA.Forum.Storage.Storages.SignOn;
using TFA.Forum.Storage.Storages.SignOut;
using TFA.Forum.Storage.Storages.Topic;

namespace TFA.Forum.Storage.DependencyInjection;

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