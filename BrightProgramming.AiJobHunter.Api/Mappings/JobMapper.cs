using BrightProgramming.AiJobHunter.Api.Controllers.Models;
using JobHunterJob = BrightProgramming.AiJobHunter.Api.Models.Job;
using PostgreSqlJob = BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL.Models.Job;

namespace BrightProgramming.AiJobHunter.Api.Mappings;

public sealed class JobMapper
{
    public JobHunterJob ToCommonJob(CreateJobRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new JobHunterJob
        {
            Title = request.Title,
            Company = request.Company,
            Url = request.Url
        };
    }

    public JobHunterJob ToCommonJob(PostgreSqlJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return new JobHunterJob
        {
            Id = job.Id,
            Title = job.Title,
            Company = job.Company,
            Url = job.Url
        };
    }

    public PostgreSqlJob ToRepositoryJob(JobHunterJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return new PostgreSqlJob
        {
            Id = job.Id,
            Title = job.Title,
            Company = job.Company,
            Url = job.Url
        };
    }
}
