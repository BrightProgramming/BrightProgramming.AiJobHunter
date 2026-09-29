using System.ComponentModel.DataAnnotations;
using BrightProgramming.AiJobHunter.Api.Controllers.Models;

namespace BrightProgramming.AiJobHunter.Api.Validations;

public sealed class CreateJobRequestValidator
{
    public IReadOnlyList<ValidationResult> Validate(CreateJobRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var results = new List<ValidationResult>();

        ValidateRequiredText(request.Title, nameof(request.Title), "Title", 200, results);
        ValidateRequiredText(request.Company, nameof(request.Company), "Company", 200, results);
        ValidateUrl(request.Url, results);

        return results;
    }

    private static void ValidateRequiredText(
        string value,
        string memberName,
        string displayName,
        int maximumLength,
        List<ValidationResult> results)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            results.Add(new ValidationResult($"{displayName} is required.", [memberName]));
        }
        else if (value.Length > maximumLength)
        {
            results.Add(new ValidationResult(
                $"{displayName} must be {maximumLength} characters or fewer.",
                [memberName]));
        }
    }

    private static void ValidateUrl(string value, List<ValidationResult> results)
    {
        const int maximumLength = 2048;
        const string memberName = nameof(CreateJobRequest.Url);

        if (string.IsNullOrWhiteSpace(value))
        {
            results.Add(new ValidationResult("URL is required.", [memberName]));
        }
        else if (value.Length > maximumLength)
        {
            results.Add(new ValidationResult(
                $"URL must be {maximumLength} characters or fewer.",
                [memberName]));
        }
        else if (!Uri.TryCreate(value, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            results.Add(new ValidationResult("URL must be an absolute HTTP or HTTPS address.", [memberName]));
        }
    }
}
