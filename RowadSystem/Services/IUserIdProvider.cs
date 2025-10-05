using Microsoft.AspNetCore.SignalR;

namespace RowadSystem.API.Services;

public interface IUserIdProvider
{
    string GetUserId(HubConnectionContext connection);
}
