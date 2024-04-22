using Microsoft.Extensions.DependencyInjection;
using TFA.Search.Domain.Models;

namespace TFA.Search.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddSearchDomain(this IServiceCollection services)
    {
        services.AddMediatR(options => 
            options
                .RegisterServicesFromAssemblyContaining<SearchEntity>());
    }
}