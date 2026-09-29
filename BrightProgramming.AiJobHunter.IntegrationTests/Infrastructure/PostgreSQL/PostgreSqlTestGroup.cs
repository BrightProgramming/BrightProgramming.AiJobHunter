namespace BrightProgramming.AiJobHunter.IntegrationTests.Infrastructure.PostgreSQL;

[CollectionDefinition(PostgreSqlDatabaseFixture.CollectionName)]
public sealed class PostgreSqlTestGroup : ICollectionFixture<PostgreSqlDatabaseFixture>
{
}
