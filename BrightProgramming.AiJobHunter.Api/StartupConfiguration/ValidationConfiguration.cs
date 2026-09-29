using BrightProgramming.AiJobHunter.Api.Validations;

namespace BrightProgramming.AiJobHunter.Api.StartupConfiguration;

public static class ValidationConfiguration
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddScoped<CreateJobRequestValidator>();
        return services;
    }
}
