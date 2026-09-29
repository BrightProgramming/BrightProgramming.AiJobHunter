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
}
