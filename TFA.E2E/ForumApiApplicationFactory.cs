using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace TFA.E2E;

public class ForumApiApplicationFactory  : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly PostgreSqlContainer _dbContainer =  new PostgreSqlBuilder().Build();
}