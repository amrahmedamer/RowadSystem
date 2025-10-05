//using Microsoft.AspNetCore.SignalR.Client;
//using RowadSystem.Shard.Contract.Notifications;

//namespace RowadSystem.UI.Features.Notifications;

//public partial class LiveNotifications
//{
//    private HubConnection hubConnection;
//    private List<NotificationDto> notifications = new();

//    protected override async Task OnInitializedAsync()
//    {
//        var auth = await AuthStateProvider.GetAuthenticationStateAsync();
//        var userId = auth.User.FindFirst("sub")?.Value;

//        hubConnection = new HubConnectionBuilder()
//            .WithUrl(NavigationManager.ToAbsoluteUri("/notificationHub"))
//            .Build();

//        hubConnection.On<string, string>("ReceiveNotification", (title, message) =>
//        {
//            notifications.Add(new NotificationDto { Title = title, Message = message, CreatedAt = DateTime.Now });
//            StateHasChanged();
//        });

//        await hubConnection.StartAsync();
//    }

//    public async ValueTask DisposeAsync()
//    {
//        await hubConnection.DisposeAsync();
//    }
//}



using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using RowadSystem.Shard.Contract.Notifications;
using RowadSystem.UI.Features.Auth;

namespace RowadSystem.UI.Features.Notifications;

public partial class LiveNotifications : IAsyncDisposable
{
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
    [Inject] private ITokenStorageService _tokenStorageService { get; set; } = default!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;

    private HubConnection hubConnection;
    private List<NotificationDto> notifications = new();

    protected override async Task OnInitializedAsync()
    {
        var auth = await AuthStateProvider.GetAuthenticationStateAsync();
        var userId = auth.User.FindFirst("sub")?.Value;

        //var token = await TokenStorage.GetAccessTokenAsync();

        //hubConnection = new HubConnectionBuilder()
        //    .WithUrl("http://localhost:5190/notificationHub", options =>
        //    {
        //        options.AccessTokenProvider = () => Task.FromResult(token);
        //    })
        //    .WithAutomaticReconnect()
        //    .Build();
        hubConnection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5190/notificationHub", options =>
    {
        options.AccessTokenProvider = async () =>
        {
            var token = await _tokenStorageService.GetAccessTokenAsync();
            Console.WriteLine("📌 Sending token: " + token);
            return token;
        };
    })
    .WithAutomaticReconnect()
    .Build();


        // استقبال رسالة خاصة
        hubConnection.On<string, string>("ReceiveMessage", (fromUserId, message) =>
        {
            notifications.Add(new NotificationDto
            {
                Title = $"رسالة من {fromUserId}",
                Message = message
            });
            InvokeAsync(StateHasChanged);
        });

        // استقبال إشعار عام
        hubConnection.On<string, string>("ReceiveNotification", (title, message) =>
        {
            notifications.Add(new NotificationDto
            {
                Title = title,
                Message = message
            });
            InvokeAsync(StateHasChanged);
        });

        await hubConnection.StartAsync();
        Console.WriteLine("✅ SignalR Connected");
    }

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }
    }
}
////using Microsoft.AspNetCore.SignalR.Client;
////using RowadSystem.Shard.Contract.Notifications;

////namespace RowadSystem.UI.Features.Notifications;

////public partial class LiveNotifications
////{
////    private HubConnection hubConnection;
////    private List<NotificationDto> notifications = new();

////    protected override async Task OnInitializedAsync()
////    {
////        var auth = await AuthStateProvider.GetAuthenticationStateAsync();
////        var userId = auth.User.FindFirst("sub")?.Value;

////        hubConnection = new HubConnectionBuilder()
////            .WithUrl(NavigationManager.ToAbsoluteUri("/notificationHub"))
////            .Build();

////        hubConnection.On<string, string>("ReceiveNotification", (title, message) =>
////        {
////            notifications.Add(new NotificationDto { Title = title, Message = message, CreatedAt = DateTime.Now });
////            StateHasChanged();
////        });

////        await hubConnection.StartAsync();
////    }

////    public async ValueTask DisposeAsync()
////    {
////        await hubConnection.DisposeAsync();
////    }
////}



//using Blazored.LocalStorage;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.SignalR.Client;
//using RowadSystem.Shard.Contract.Notifications;
//using RowadSystem.UI.Features.Auth;

//namespace RowadSystem.UI.Features.Notifications;

//public partial class LiveNotifications : IAsyncDisposable
//{
//    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
//    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
//    [Inject] private ITokenStorageService _tokenStorageService { get; set; } = default!;
//    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!; 
//    private HubConnection hubConnection;
//    private List<NotificationDto> notifications = new();

//    protected override async Task OnInitializedAsync()
//    {
//        var auth = await AuthStateProvider.GetAuthenticationStateAsync();
//        var userId = auth.User.FindFirst("sub")?.Value;

//        hubConnection = new HubConnectionBuilder()
//            .WithUrl("http://localhost:5190/notificationHub", options =>
//            {
//                // ✅ لو عندك JWT Authentication
//                //options.AccessTokenProvider = async () => await Blazored.LocalStorage.GetItemAsync<string>("access_token");
//                //options.AccessTokenProvider = async () => await _tokenStorageService.GetAccessTokenAsync();
//                options.AccessTokenProvider = async () =>
//                {
//                    // هنا بتقرأ التوكن من LocalStorage
//                     return await _tokenStorageService.GetAccessTokenAsync();
//                    //Console.WriteLine("token  : " + LocalStorage.GetItemAsync<string>("access_token"));
//                };

//            })
//            .WithAutomaticReconnect() // علشان لو النت قطع يرجع يتصل تاني
//            .Build();

//        hubConnection.On<string, string>("ReceiveNotification", (title, message) =>
//        {
//            Console.WriteLine($"Connected User: {userId}");
//            notifications.Add(new NotificationDto
//            {
//                Title = title,
//                Message = message,
//                //CreatedAt = DateTime.Now
//            });
//            Console.WriteLine($"Notification received: {title} - {message}");
//            InvokeAsync(StateHasChanged); // ✅ أفضل من StateHasChanged بس
//        });

//        await hubConnection.StartAsync();
//        Console.WriteLine("SignalR connected!");
//    }

//    public async ValueTask DisposeAsync()
//    {
//        if (hubConnection is not null)
//        {
//            await hubConnection.DisposeAsync();
//        }
//    }
//}
