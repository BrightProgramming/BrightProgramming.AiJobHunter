using BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL.Models;

namespace BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL.Repositories;

public interface IJobRepository
{
    Task SaveAsync(Job job, CancellationToken cancellationToken);
}
