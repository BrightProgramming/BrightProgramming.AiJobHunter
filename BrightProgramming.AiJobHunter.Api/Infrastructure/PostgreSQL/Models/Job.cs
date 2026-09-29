namespace BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL.Models;

public sealed class Job
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Title { get; init; } = string.Empty;

    public string Company { get; init; } = string.Empty;

    public string Url { get; init; } = string.Empty;
}
