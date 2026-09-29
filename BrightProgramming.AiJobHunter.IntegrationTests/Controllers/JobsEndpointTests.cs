using System.Net;
using System.Net.Http.Json;
using BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL;
using BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL.Repositories;
using BrightProgramming.AiJobHunter.Api.Models;
using BrightProgramming.AiJobHunter.IntegrationTests.Infrastructure.PostgreSQL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Npgsql;
using PostgreSqlJob = BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL.Models.Job;

namespace BrightProgramming.AiJobHunter.IntegrationTests.Controllers;

[Collection(PostgreSqlDatabaseFixture.CollectionName)]
public sealed class JobsEndpointTests(
    WebApplicationFactory<Program> factory,
    PostgreSqlDatabaseFixture databaseFixture)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetJobsReturnsEmptyArrayWhenDatabaseHasNoJobs()
    {
        await ClearJobsAsync();
        using var client = CreateClient();

        var response = await client.GetAsync("/jobs");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jobs = await response.Content.ReadFromJsonAsync<Job[]>();
        Assert.NotNull(jobs);
        Assert.Empty(jobs);
    }

    [Fact]
    public async Task GetJobsReturnsJobsStoredInPostgreSql()
    {
        await ClearJobsAsync();
        var repositoryJob = new PostgreSqlJob
        {
            Title = "Platform Engineer",
            Company = "Example Co",
            Url = "https://example.com/jobs/123"
        };
        await using (var dataSource = NpgsqlDataSource.Create(databaseFixture.ConnectionString))
        {
            var repository = new JobRepository(new DatabaseContext(dataSource));
            await repository.SaveAsync(repositoryJob, CancellationToken.None);
        }

        using var client = CreateClient();
        var response = await client.GetAsync("/jobs");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jobs = await response.Content.ReadFromJsonAsync<Job[]>();
        var job = Assert.Single(jobs!);
        Assert.Equal(repositoryJob.Id, job.Id);
        Assert.Equal(repositoryJob.Title, job.Title);
        Assert.Equal(repositoryJob.Company, job.Company);
        Assert.Equal(repositoryJob.Url, job.Url);
    }

    private HttpClient CreateClient() => factory
        .WithWebHostBuilder(builder =>
        {
            builder.ConfigureLogging(logging => logging.ClearProviders());
            builder.ConfigureAppConfiguration((_, configuration) =>
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:PostgreSQL"] = databaseFixture.ConnectionString,
                    ["Database:MigrateOnStartup"] = "false"
                }));
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<NpgsqlDataSource>();
                services.AddSingleton(NpgsqlDataSource.Create(databaseFixture.ConnectionString));
            });
        })
        .CreateClient();

    private async Task ClearJobsAsync()
    {
        await using var dataSource = NpgsqlDataSource.Create(databaseFixture.ConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();
        await using var command = new NpgsqlCommand("TRUNCATE TABLE jobs;", connection);
        await command.ExecuteNonQueryAsync();
    }
}
