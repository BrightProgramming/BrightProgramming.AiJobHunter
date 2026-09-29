using BrightProgramming.AiJobHunter.Api.Controllers.Models;
using BrightProgramming.AiJobHunter.Api.Validations;

namespace BrightProgramming.AiJobHunter.UnitTests.Validation;

public sealed class CreateJobRequestValidatorTests
{
    private readonly CreateJobRequestValidator _validator = new();

    [Fact]
    public void ValidRequestPassesValidation()
    {
        var request = new CreateJobRequest
        {
            Title = "Software Engineer",
            Company = "Bright Programming",
            Url = "https://example.com/jobs/123"
        };

        Assert.Empty(_validator.Validate(request));
    }

    [Fact]
    public void MissingTitleFailsValidation()
    {
        var request = new CreateJobRequest
        {
            Company = "Bright Programming",
            Url = "https://example.com/jobs/123"
        };

        Assert.Contains(_validator.Validate(request), result => result.MemberNames.Contains(nameof(CreateJobRequest.Title)));
    }

    [Fact]
    public void MissingCompanyFailsValidation()
    {
        var request = new CreateJobRequest
        {
            Title = "Software Engineer",
            Url = "https://example.com/jobs/123"
        };

        Assert.Contains(_validator.Validate(request), result => result.MemberNames.Contains(nameof(CreateJobRequest.Company)));
    }

    [Fact]
    public void InvalidUrlFailsValidation()
    {
        var request = new CreateJobRequest
        {
            Title = "Software Engineer",
            Company = "Bright Programming",
            Url = "not-a-url"
        };

        Assert.Contains(_validator.Validate(request), result => result.MemberNames.Contains(nameof(CreateJobRequest.Url)));
    }
}
