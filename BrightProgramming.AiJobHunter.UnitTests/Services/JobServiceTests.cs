using BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL.Repositories;
using BrightProgramming.AiJobHunter.Api.Mappings;
using BrightProgramming.AiJobHunter.Api.Services;
using Moq;
using CommonJob = BrightProgramming.AiJobHunter.Api.Models.Job;
using RepositoryJob = BrightProgramming.AiJobHunter.Api.Infrastructure.PostgreSQL.Models.Job;

namespace BrightProgramming.AiJobHunter.UnitTests.Services;

public sealed class JobServiceTests
{
    [Fact]
    public async Task SaveAsyncCallsRepositoryWithMappedJob()
    {
        var repository = new Mock<IJobRepository>();
        RepositoryJob? savedJob = null;
        repository
            .Setup(mock => mock.SaveAsync(
                It.IsAny<RepositoryJob>(),
                It.IsAny<CancellationToken>()))
            .Callback<RepositoryJob, CancellationToken>((job, _) => savedJob = job)
            .Returns(Task.CompletedTask);

        var service = new JobService(repository.Object, new JobMapper());
        var job = new CommonJob
        {
            Title = "Platform Engineer",
            Company = "Example Co",
            Url = "https://example.com/jobs/123"
        };
        using var cancellationTokenSource = new CancellationTokenSource();

        var savedJobId = await service.SaveAsync(job, cancellationTokenSource.Token);

        repository.Verify(mock => mock.SaveAsync(
            It.Is<RepositoryJob>(repositoryJob =>
                repositoryJob.Title == job.Title
                && repositoryJob.Company == job.Company
                && repositoryJob.Url == job.Url),
            cancellationTokenSource.Token), Times.Once);

        Assert.NotNull(savedJob);
        Assert.Equal(savedJob!.Id, savedJobId);
    }

    [Fact]
    public async Task GetAllAsyncReturnsMappedJobsFromRepository()
    {
        var repository = new Mock<IJobRepository>();
        var repositoryJob = new RepositoryJob
        {
            Id = Guid.NewGuid(),
            Title = "Platform Engineer",
            Company = "Example Co",
            Url = "https://example.com/jobs/123"
        };
        repository
            .Setup(mock => mock.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { repositoryJob });

        var service = new JobService(repository.Object, new JobMapper());
        using var cancellationTokenSource = new CancellationTokenSource();

        var jobs = await service.GetAllAsync(cancellationTokenSource.Token);

        repository.Verify(mock => mock.GetAllAsync(cancellationTokenSource.Token), Times.Once);
        var job = Assert.Single(jobs);
        Assert.Equal(repositoryJob.Id, job.Id);
        Assert.Equal(repositoryJob.Title, job.Title);
        Assert.Equal(repositoryJob.Company, job.Company);
        Assert.Equal(repositoryJob.Url, job.Url);
    }
}
