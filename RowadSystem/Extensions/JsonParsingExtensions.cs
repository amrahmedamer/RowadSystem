using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace RowadSystem.Extensions;

public static class JsonParsingExtensions
{
    public static (T? result, IActionResult? errorResult) TryParseJson<T>(this string json, ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return (default, new BadRequestObjectResult(new
            {
                error = "Invalid JSON format in 'data'",
                details = "Empty or whitespace JSON string."
            }));
        }

        try
        {
            var result = JsonConvert.DeserializeObject<T>(json);
            return (result, null);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to parse JSON: {Json}", json);
            return (default, new BadRequestObjectResult(new
            {
                error = "Invalid JSON format in 'data'",
                details = "The input could not be parsed. Please check the JSON format."
            }));
        }
    }

}
