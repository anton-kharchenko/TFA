using Microsoft.Extensions.DependencyInjection;
using OpenSearch.Client;
using TFA.Search.Domain.Contracts;
using TFA.Search.Storage.Entities;
using TFA.Search.Storage.Storages;

namespace TFA.Search.Storage.DependencyInjection;

public static class SearchCollectionExtensions
{
    public static void AddSearchStorage(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection
            .AddScoped<IIndexStorage, IndexStorage>()
            .AddScoped<ISearchStorage, SearchStorage>();

        serviceCollection.AddSingleton<IOpenSearchClient>(new OpenSearchClient(new Uri(connectionString))
        {
            ConnectionSettings =
            {
                DefaultIndices = { [typeof(SearchEntity)] = "tfa-search-v1" },
            }
        });
    }
}