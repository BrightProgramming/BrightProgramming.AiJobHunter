using BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL.Models;
using Npgsql;

namespace BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL.Repositories;

public sealed class JobRepository : IJobRepository
{
    private readonly DatabaseContext _databaseContext;

    public JobRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task SaveAsync(Job job, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(job);

        const string sql = """
            INSERT INTO jobs (id, title, company, url)
            VALUES (@id, @title, @company, @url);
            """;

        await using var connection = await _databaseContext.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("id", job.Id);
        command.Parameters.AddWithValue("title", job.Title);
        command.Parameters.AddWithValue("company", job.Company);
        command.Parameters.AddWithValue("url", job.Url);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Job>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string sql = "SELECT id, title, company, url FROM jobs;";

        await using var connection = await _databaseContext.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var jobs = new List<Job>();
        while (await reader.ReadAsync(cancellationToken))
        {
            jobs.Add(new Job
            {
                Id = reader.GetGuid(0),
                Title = reader.GetString(1),
                Company = reader.GetString(2),
                Url = reader.GetString(3)
            });
        }

        return jobs;
    }
}
