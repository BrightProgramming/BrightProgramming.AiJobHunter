using BrightProgramming.AiJobHunter.Api.StartupConfiguration;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace BrightProgramming.AiJobHunter.IntegrationTests.Infrastructure.PostgreSQL;

public sealed class PostgreSqlDatabaseFixture : IAsyncLifetime
{
    public const string CollectionName = "PostgreSQL integration tests";

    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:17-alpine")
        .WithDatabase("aijobhunter_integrationtests")
        .WithUsername("aijobhunter_test")
        .WithPassword("integration-test-only")
        .Build();

    public string ConnectionString => _container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:PostgreSQL"] = ConnectionString,
                ["Database:MigrateOnStartup"] = "true"
            })
            .Build();

        DatabaseConfiguration.ApplyPostgreSqlMigrations(configuration);
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}
