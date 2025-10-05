using RowadSystem.Shard.Abstractions;
using System.Net.Http.Json;
using System.Text.Json;

namespace RowadSystem.UI.Features;

public static class PorblemDetailsExtensions
{

    public static async Task<Result<T>> HandleErrorResponse<T>(HttpResponseMessage response)
    {
        Console.WriteLine($"Handling error response with status code: {response.StatusCode}");
        try
        {
            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(problemDetails))
                    return Result.Failure<T>(new Error("EmptyResponse", "The server returned an empty response.", 500));


                var jsonDoc = JsonDocument.Parse(problemDetails);
                Console.WriteLine("Parsed JSON document successfully."+ jsonDoc);

                //var type = jsonDoc.RootElement.GetProperty("type").GetString();
                //var title = jsonDoc.RootElement.GetProperty("title").GetString();
                //var status = jsonDoc.RootElement.GetProperty("status").GetInt32();
                //var traceId = jsonDoc.RootElement.GetProperty("traceId").GetString();

                var error = jsonDoc.RootElement.GetProperty("error");
                var code = error.GetProperty("code").GetString();
                Console.WriteLine($"Error:code: {code} ");

                var description = error.GetProperty("description").GetString();
                Console.WriteLine($"Error: description : {description} ");

                var statusCode = error.GetProperty("statusCode").GetInt32();
                Console.WriteLine($"Error: statusCode : {statusCode}");


                return Result.Failure<T>(new Error(code, description, statusCode));
            }
            Console.WriteLine($"Error: UnknownError");

            return Result.Failure<T>(new Error("UnknownError", "No valid data returned.", 500));
        }
        catch (Exception ex)
        {
            return Result.Failure<T>(new Error("UnknownError", ex.Message, 500));
        }
    }
    //public static async Task<Result<T>> HandleErrorResponse<T>(HttpResponseMessage response)
    //{
    //    Console.WriteLine($"Handling error response with status code: {response.StatusCode}");

    //    try
    //    {
    //        if (!response.IsSuccessStatusCode)
    //        {
    //            Console.WriteLine("Response is not successful, reading problem details...");
    //            var problemDetails = await response.Content.ReadAsStringAsync();
    //            var jsonDoc = JsonDocument.Parse(problemDetails);

    //            // Safely handle errors using TryGetProperty
    //            var error = jsonDoc.RootElement.GetProperty("error");

    //            string code = error.GetProperty("code").GetString();
    //            string description = error.GetProperty("description").GetString();
    //            int statusCode = error.GetProperty("statusCode").GetInt32();

    //            Console.WriteLine($"Error: {code} - {description} - {statusCode}");

    //            return Result.Failure<T>(new Error(code, description, statusCode));
    //        }

    //        return Result.Failure<T>(new Error("UnknownError", "No valid data returned.", 500));
    //    }
    //    catch (Exception ex)
    //    {
    //        return Result.Failure<T>(new Error("UnknownError", ex.Message, 500));
    //    }
    //}


}
