using DbUp;
using BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL;
using Npgsql;
using System.Reflection;

namespace BrightProgramming.AiJobHunter.Api.StartupConfiguration;

public static class DatabaseConfiguration
{
    public static void ApplyPostgreSqlMigrations(IConfiguration configuration)
    {
        if (!configuration.GetValue<bool>("Database:MigrateOnStartup"))
        {
            return;
        }

        var connectionString = configuration.GetConnectionString("PostgreSQL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("The PostgreSQL connection string is not configured.");
        }

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();
        if (!result.Successful)
        {
            throw new InvalidOperationException("Database migration failed.", result.Error);
        }
    }

    public static IServiceCollection AddPostgreSql(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSQL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("The PostgreSQL connection string is not configured.");
        }

        services.AddSingleton(_ => NpgsqlDataSource.Create(connectionString));
        services.AddScoped<DatabaseContext>();
        return services;
    }
}
