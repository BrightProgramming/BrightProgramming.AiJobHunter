namespace BrightProgramming.AiJobHunter.Api.Controllers.Models;

public sealed class CreateJobRequest
{
    public string Title { get; init; } = string.Empty;

    public string Company { get; init; } = string.Empty;

    public string Url { get; init; } = string.Empty;
}
