using Microsoft.AspNetCore.Mvc;
using RowadSystem.API.Services;
using RowadSystem.Shard.Contract.Notifications;
using System.IdentityModel.Tokens.Jwt;

namespace RowadSystem.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationsController ( NotificationService notificationService) : ControllerBase
{
    //private readonly ApplicationDbContext _context = context;
    private readonly NotificationService _notificationService = notificationService;
   

    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] NotificationDto dto)
    {
        var userId = User.GetUserId();
        //var fromUserId = User.FindFirst(JwtRegisteredClaimNames.Sub).Value;

       
        // تخزين في DB
        //var notification = new Notification
        //{
        //    UserId = dto.UserId,
        //    Title = dto.Title,
        //    Message = dto.Message,
        //    CreatedAt = DateTime.Now,
        //    IsRead = false
        //};
        //_context.Add(notification);
        //await _context.SaveChangesAsync();

        // إرسال فوري
        //await hub.Clients.All.SendAsync("ReceiveNotification", dto.Title, dto.Message);
        //await hub.Clients.User(dto.UserId).SendAsync("ReceiveNotification", dto.Title, dto.Message);
        await _notificationService.SendNotification(userId, dto.UserId, dto.Message);
        //await _notificationService.SendNotification(dto.UserId, dto.Title, dto.Message);
        //Console.WriteLine($"Sent notification to {dto.UserId}: {dto.Title}");
        return Ok();
    }
    [HttpPost("send-all-users")]
    public async Task<IActionResult> SendNotificationAllUsers([FromBody] NotificationDto dto)
    {

        await _notificationService.SendNotificationAllUsers(dto.UserId,dto.Title, dto.Message);
        return Ok();
    }

}
