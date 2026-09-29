using BrightProgramming.AiJobHunter.Api.Controllers.Models;
using BrightProgramming.AiJobHunter.Api.Mappings;
using CommonJob = BrightProgramming.AiJobHunter.Api.Models.Job;

namespace BrightProgramming.AiJobHunter.UnitTests.Mappings;

public sealed class JobMapperTests
{
    private readonly JobMapper _mapper = new();

    [Fact]
    public void ToCommonJobMapsCreateJobRequestProperties()
    {
        var request = new CreateJobRequest
        {
            Title = "Platform Engineer",
            Company = "Example Co",
            Url = "https://example.com/jobs/123"
        };

        var job = _mapper.ToCommonJob(request);

        Assert.Equal(request.Title, job.Title);
        Assert.Equal(request.Company, job.Company);
        Assert.Equal(request.Url, job.Url);
    }

    [Fact]
    public void ToRepositoryJobMapsCommonJobProperties()
    {
        var job = new CommonJob
        {
            Title = "Platform Engineer",
            Company = "Example Co",
            Url = "https://example.com/jobs/123"
        };

        var repositoryJob = _mapper.ToRepositoryJob(job);

        Assert.Equal(job.Title, repositoryJob.Title);
        Assert.Equal(job.Company, repositoryJob.Company);
        Assert.Equal(job.Url, repositoryJob.Url);
        Assert.NotEqual(Guid.Empty, repositoryJob.Id);
    }
}
