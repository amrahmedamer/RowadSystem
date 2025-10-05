using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace RowadSystem.API.Services;

//public class NotificationService : Hub
//{
//    public async Task SendNotification(string userId, string title, string message)
//    {
//        await Clients.User(userId).SendAsync("ReceiveNotification", title, message);
//    }
//}

public class NotificationService : Hub
{
    private readonly IHubContext<NotificationService> _hub;

    public NotificationService(IHubContext<NotificationService> hub)
    {
        _hub = hub;
    }
     public override async Task OnConnectedAsync()
    {
        Console.WriteLine("✅ Hub Connected");
        Console.WriteLine("User Identity: " + Context.User?.Identity?.Name);
        foreach (var claim in Context.User?.Claims ?? Enumerable.Empty<Claim>())
        {
            Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
        }
        await base.OnConnectedAsync();
    }
    public async Task SendNotification(string fromUserId, string toUserId, string message)
    {
        //var userId = Context.UserIdentifier;
        //await _hub.Clients.User(userId).SendAsync("ReceiveNotification", title, message);
        //Console.WriteLine($"from={userId}, to={toUserId}, message={message}");

        await _hub.Clients.User(toUserId).SendAsync("ReceiveMessage", fromUserId, message);
    }
    public async Task SendNotificationAllUsers(string userId, string title, string message)
    {
        await _hub.Clients.All.SendAsync("ReceiveNotification", title, message);
    }
}
