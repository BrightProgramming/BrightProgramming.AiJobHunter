using BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL;
using BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL.Repositories;
using Npgsql;
using PostgreSqlJob = BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL.Models.Job;

namespace BrightProgramming.AiJobHunter.IntegrationTests.Infrastructure.PostgreSQL.Repositories;

[Collection(PostgreSqlDatabaseFixture.CollectionName)]
public sealed class JobRepositoryTests(PostgreSqlDatabaseFixture databaseFixture)
{
    [Fact]
    public async Task SaveAsyncPersistsJobToPostgreSql()
    {
        await using var dataSource = NpgsqlDataSource.Create(databaseFixture.ConnectionString);
        var repository = new JobRepository(new DatabaseContext(dataSource));
        var job = new PostgreSqlJob
        {
            Title = "Platform Engineer",
            Company = "Example Co",
            Url = "https://example.com/jobs/123"
        };

        await repository.SaveAsync(job, CancellationToken.None);

        await using var connection = await dataSource.OpenConnectionAsync();
        await using var command = new NpgsqlCommand(
            "SELECT id, title, company, url FROM jobs WHERE id = @id;",
            connection);
        command.Parameters.AddWithValue("id", job.Id);

        await using var reader = await command.ExecuteReaderAsync();
        Assert.True(await reader.ReadAsync());
        Assert.Equal(job.Id, reader.GetGuid(0));
        Assert.Equal(job.Title, reader.GetString(1));
        Assert.Equal(job.Company, reader.GetString(2));
        Assert.Equal(job.Url, reader.GetString(3));
        Assert.False(await reader.ReadAsync());
    }
}
