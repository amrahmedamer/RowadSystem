using Microsoft.AspNetCore.SignalR;


namespace RowadSystem.API.Services;

public class CustomUserIdProvider: IUserIdProvider
{
    //public string GetUserId(HubConnectionContext connection)
    //{
    //    //return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //    Console.WriteLine("sub : " + connection.User?.FindFirst("sub")?.Value);
    //   return connection.User?.FindFirst("sub")?.Value;
    //}
    public string GetUserId(HubConnectionContext connection)
    {
        var userId = connection.User?.FindFirst("sub")?.Value;

        Console.WriteLine($"🔑 Extracted UserId from JWT: {userId}");

        return userId; // SignalR هيستخدمه كـ Context.UserIdentifier
    }
}
