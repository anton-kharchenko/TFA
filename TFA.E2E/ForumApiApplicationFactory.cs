using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;
using TFA.Storage.Configurations;

namespace TFA.E2E;

public class ForumApiApplicationFactory  : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer =  new PostgreSqlBuilder().Build();
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        var forumDbContext = new ForumDbContext(new DbContextOptionsBuilder<ForumDbContext>()
            .UseNpgsql(_dbContainer.GetConnectionString()).Options);
        await forumDbContext.Database.MigrateAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:Postgres"] = _dbContainer.GetConnectionString(),
            }!)
            .Build();
        builder.UseConfiguration(configuration);
        base.ConfigureWebHost(builder);
    }

    public new async Task DisposeAsync() => await _dbContainer.DisposeAsync();
}