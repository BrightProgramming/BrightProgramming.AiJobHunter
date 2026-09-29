using BrightProgramming.AiJobHunter.Api.Controllers.Models;
using BrightProgramming.AiJobHunter.Api.Mappings;
using BrightProgramming.AiJobHunter.Api.Services;
using BrightProgramming.AiJobHunter.Api.Validations;
using Microsoft.AspNetCore.Mvc;
using CommonJob = BrightProgramming.AiJobHunter.Api.Models.Job;

namespace BrightProgramming.AiJobHunter.Api.Controllers;

[ApiController]
[Route("jobs")]
public sealed class JobsController(
    IJobService jobService,
    JobMapper jobMapper,
    CreateJobRequestValidator validator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CommonJob>>> GetAll(CancellationToken cancellationToken)
    {
        var jobs = await jobService.GetAllAsync(cancellationToken);
        return Ok(jobs);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateJobRequest request,
        CancellationToken cancellationToken)
    {
        var validationResults = validator.Validate(request);
        if (validationResults.Count > 0)
        {
            foreach (var validationResult in validationResults)
            {
                var memberNames = validationResult.MemberNames.DefaultIfEmpty(string.Empty);
                foreach (var memberName in memberNames)
                {
                    ModelState.AddModelError(memberName, validationResult.ErrorMessage ?? "The request is invalid.");
                }
            }

            return ValidationProblem(ModelState);
        }

        var job = jobMapper.ToCommonJob(request);

        var jobId = await jobService.SaveAsync(job, cancellationToken);

        return Created($"/jobs/{jobId}", new { Id = jobId });
    }
}
