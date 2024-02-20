using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TFA.Domain.Helpers;
using TFA.Domain.Interfaces.Helpers;
using TFA.Domain.Interfaces.Storages;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Storage.Storages;

namespace TFA.Storage.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddForumStorages(this IServiceCollection services, string? connectionString = default)
    {
        services
         .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
         .AddScoped<IGetForumsStorage, GetForumsStorage>();
            
        services.AddScoped<IGuidFactory, GuidFactory>();
        services.AddScoped<IMomentProvider, MomentProvider>();
        
        services.AddDbContextPool<ForumDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });
    }
}