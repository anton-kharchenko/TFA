using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Testcontainers.PostgreSql;
using TFA.Storage.Configurations;

namespace TFA.Storage.Tests;

public class StorageTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();

    public IMapper GetMapper() => new Mapper(new MapperConfiguration(cfg =>
        cfg.AddMaps(Assembly.GetAssembly(typeof(ForumDbContext)))));

    public IMemoryCache GetMemoryCache() => new MemoryCache(new MemoryCacheOptions());

    public ForumDbContext GetDbContext() => new(new DbContextOptionsBuilder<ForumDbContext>()
        .UseNpgsql(_dbContainer.GetConnectionString()).Options);

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        var forumDbContext = new ForumDbContext(new DbContextOptionsBuilder<ForumDbContext>()
            .UseNpgsql(_dbContainer.GetConnectionString()).Options);
        await forumDbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync() => await _dbContainer.DisposeAsync();
}