using BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL.Repositories;
using BrightProgramming.AiJobHunter.Api.Mappings;
using BrightProgramming.AiJobHunter.Api.Services;

namespace BrightProgramming.AiJobHunter.Api.StartupConfiguration;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IJobService, JobService>();
        services.AddSingleton<JobMapper>();
        return services;
    }
}
