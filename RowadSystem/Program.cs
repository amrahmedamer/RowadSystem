using RowadSystem;
using RowadSystem.API.Services;
using RowadSystem.Middleware;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Dependencies(builder);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy => policy
            .AllowAnyOrigin()  // أو UseWithOrigins("https://localhost:xxxx")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler();
//app.UseHttpsRedirection();
app.UseCors("AllowBlazor");

app.UseMiddleware<GuestIdMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationService>("/notificationHub");

app.MapControllers();

app.Run();
