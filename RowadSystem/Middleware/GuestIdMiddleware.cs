namespace RowadSystem.Middleware;

public class GuestIdMiddleware
{
    private readonly RequestDelegate _next;

    public GuestIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Cookies.ContainsKey("GuestId"))
        {
            var guestId = Guid.NewGuid().ToString();
            context.Response.Cookies.Append("GuestId", guestId, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(30),
                IsEssential = true
            });
        }

        await _next(context);
    }
}

