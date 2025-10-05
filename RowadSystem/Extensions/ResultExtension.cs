using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace RowadSystem.Abstractions;

public static class ResultExtension
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new Exception("Complaint details cannot be returned in the success case.");

        var problemDetails = new ProblemDetails
        {
            Type = $"https://tools.ietf.org/html/rfc7231#section-6.{result.Error.statusCode / 100}.{result.Error.statusCode % 100}",
            Title = ReasonPhrases.GetReasonPhrase(result.Error.statusCode),
            Status = result.Error.statusCode,
            Extensions = new Dictionary<string, object?>
            {
                { "error", result.Error },
                { "traceId", Guid.NewGuid().ToString() }
            }
        };

        return new ObjectResult(problemDetails);
    }
}
