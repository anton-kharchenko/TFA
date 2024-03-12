using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TFA.Domain.Interfaces.Storages.Topic;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Interfaces.UseCases.GetTopics;
using TFA.Storage.Helpers;
using TFA.Storage.Storages;
using TFA.Storage.Storages.Forum;
using TFA.Storage.Storages.Topic;

namespace TFA.Storage.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddForumStorages(this IServiceCollection services, string? connectionString = default)
    {
        services
         .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
         .AddScoped<IGetForumsStorage, GetForumsStorage>()
         .AddScoped<IGetTopicsStorage, GetTopicStorage>();
            
        services.AddScoped<IGuidFactory, GuidFactory>();
        services.AddScoped<IMomentProvider, MomentProvider>();
        
        services.AddDbContextPool<ForumDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });

        services.AddMemoryCache();
    }
}