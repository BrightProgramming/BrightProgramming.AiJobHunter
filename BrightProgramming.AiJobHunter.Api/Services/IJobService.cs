using JobHunterJob = BrightProgramming.AiJobHunter.Api.Models.Job;

namespace BrightProgramming.AiJobHunter.Api.Services;

public interface IJobService
{
    Task<Guid> SaveAsync(JobHunterJob job, CancellationToken cancellationToken);

    Task<IReadOnlyList<JobHunterJob>> GetAllAsync(CancellationToken cancellationToken);
}
