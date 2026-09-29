using BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL.Repositories;
using BrightProgramming.AiJobHunter.Api.Mappings;
using JobHunterJob = BrightProgramming.AiJobHunter.Api.Models.Job;

namespace BrightProgramming.AiJobHunter.Api.Services;

public sealed class JobService(IJobRepository jobRepository, JobMapper jobMapper) : IJobService
{
    public async Task<Guid> SaveAsync(JobHunterJob job, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(job);

        var repositoryJob = jobMapper.ToRepositoryJob(job);

        await jobRepository.SaveAsync(repositoryJob, cancellationToken);

        return repositoryJob.Id;
    }
}
