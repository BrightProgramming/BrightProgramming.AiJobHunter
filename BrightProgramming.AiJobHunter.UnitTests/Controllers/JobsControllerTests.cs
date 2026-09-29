using BrightProgramming.AiJobHunter.Api.Controllers;
using BrightProgramming.AiJobHunter.Api.Controllers.Models;
using BrightProgramming.AiJobHunter.Api.Mappings;
using BrightProgramming.AiJobHunter.Api.Services;
using BrightProgramming.AiJobHunter.Api.Validations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using CommonJob = BrightProgramming.AiJobHunter.Api.Models.Job;

namespace BrightProgramming.AiJobHunter.UnitTests.Controllers;

public sealed class JobsControllerTests
{
    [Fact]
    public async Task CreateValidRequestCallsServiceWithMappedJob()
    {
        var service = new Mock<IJobService>();
        var jobId = Guid.NewGuid();
        service.Setup(mock => mock.SaveAsync(It.IsAny<CommonJob>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(jobId);
        var controller = CreateController(service.Object);
        var request = new CreateJobRequest
        {
            Title = "Platform Engineer",
            Company = "Example Co",
            Url = "https://example.com/jobs/123"
        };
        using var cancellationTokenSource = new CancellationTokenSource();

        var result = await controller.Create(request, cancellationTokenSource.Token);

        service.Verify(mock => mock.SaveAsync(
            It.Is<CommonJob>(job =>
                job.Title == request.Title
                && job.Company == request.Company
                && job.Url == request.Url),
            cancellationTokenSource.Token), Times.Once);

        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal($"/jobs/{jobId}", createdResult.Location);
    }

    [Fact]
    public async Task CreateInvalidRequestReturnsBadRequestWithoutCallingService()
    {
        var service = new Mock<IJobService>();
        var controller = CreateController(service.Object);
        var request = new CreateJobRequest
        {
            Title = "Platform Engineer",
            Company = "Example Co",
            Url = "not-a-url"
        };

        var result = await controller.Create(request, CancellationToken.None);

        var badRequest = Assert.IsType<ObjectResult>(result);
        var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequest.Value);
        Assert.Contains(nameof(CreateJobRequest.Url), problemDetails.Errors.Keys);
        service.Verify(mock => mock.SaveAsync(
            It.IsAny<CommonJob>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    private static JobsController CreateController(IJobService jobService) =>
        new(jobService, new JobMapper(), new CreateJobRequestValidator());
}
