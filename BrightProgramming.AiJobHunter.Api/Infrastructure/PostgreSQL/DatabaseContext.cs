using Npgsql;

namespace BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL;

public sealed class DatabaseContext(NpgsqlDataSource dataSource)
{
    private readonly NpgsqlDataSource _dataSource = dataSource;

    public ValueTask<NpgsqlConnection> OpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        return _dataSource.OpenConnectionAsync(cancellationToken);
    }
}
